using api.v1.main.DTOs.Box;

using component.v1.exceptions;

using db.v1.main.DTOs.Box;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.User;
using db.v1.main.DTOs.BoxType;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.regex.Interfaces;
using helper.v1.time;
using helper.v1.localization.Helper;
using helper.v1.file;
using helper.v1.messageBroker;

namespace api.v1.main.Services.Box
{
    public sealed class BoxService(IBoxRepository box, IUserRepository user, ICacheHelper cache,
        IFileConfigurationHelper fileCfg, IFileHelper file, IBoxRegexHelper rgx, ITimeHelper time, ILocalizationHelper localization, 
        IActivityConfigurationHelper activityCfg, IMessageBrokerHelper broker, ICacheConfigurationHelper cacheCfg) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IBoxService
    {
        private readonly IBoxRepository _box = box;

        private readonly IBoxRegexHelper _rgx = rgx;
        private readonly ITimeHelper _time = time;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;

        public async Task AddBox(PostBoxDTO body, Guid statsID)
        {
            ValidateUserID(body.UserID);
            ValidateBoxType(body.TypeID);
            ValidateBoxTitle(body.UserID, body.Title);

            var names = UploadFile(body.File, body.Preview, body.UserID, body.Title!, _fileCfg.GetBoxFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertBoxBody = new InsertBoxDTO(body.UserID, body.TypeID, body.Title!, names.FileName, names.PreviewName, currentTime);
            _box.InsertBoxInfo(insertBoxBody);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task UpdateBox(PutBoxDTO body, Guid statsID)
        {
            ValidateBoxTitle(body.UserID, body.Title);
            ValidateBoxOwner(body.BoxID, body.UserID);

            var names = UpdateFile(body.File, body.Preview, body.UserID, body.BoxID, body.Title!, 
                _box.SelectBoxFileName!, _box.SelectBoxPreviewName!, _fileCfg.GetBoxFilePath);

            var updateBoxBody = new UpdateBoxDTO(body.BoxID, body.Title!, names.FileName, names.PreviewName);
            _box.UpdateBoxInfo(updateBoxBody);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task PatchBoxTitle(PatchBoxTitleDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateBoxID(body.BoxID);
            ValidateBoxTitle(userID, body.Title);
            ValidateBoxOwner(body.BoxID, userID);

            _box.UpdateBoxTitle(body.Title, body.BoxID);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task DeleteBox(DeleteBoxDTO body, Guid userID, Guid statsID)
        {
            ValidateBoxOwner(body.BoxID, userID);

            var modelFileName = _box.SelectBoxFileName(body.BoxID)!;
            var modelFilePath = _fileCfg.GetBoxFilePath(userID, modelFileName);

            var imgFileName = _box.SelectBoxPreviewName(body.BoxID)!;
            var imgFilePath = _fileCfg.GetBoxFilePath(userID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _cache.DeleteValue(modelFilePath);
            _cache.DeleteValue(imgFilePath);
            _box.DeleteBoxInfo(body.BoxID);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }



        public async Task<byte[]> GetBoxFileBytes(Guid boxID, Guid statsID)
        {
            ValidateBoxID(boxID);
            var fileName = _box.SelectBoxFileName(boxID);
            var userID = _box.SelectBoxOwnerID(boxID);
            var filePath = _fileCfg.GetBoxFilePath((Guid)userID!, fileName!);

            var file = await ReadFile(filePath);

            await PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return file;
        }

        public async Task<string> GetBoxBase64Preview(Guid boxID)
        {
            ValidateBoxID(boxID);
            var previewName = _box.SelectBoxPreviewName(boxID);
            var userID = _box.SelectBoxOwnerID(boxID);
            var previewPath = _fileCfg.GetBoxFilePath((Guid)userID!, previewName!);

            var preview = await ReadFile(previewPath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectBoxDTO>> GetDefaultBoxesList(BoxPaginationDTO body, Guid statsID)
        {
            ValidateBoxType(body.TypeID);
            var userID = _fileCfg.GetDefaultModelsUserID();

            var boxes = GetPaginationListOfObjects(body.Page, body.PageSize, body.TypeID, userID, _box.SelectUserBoxes);

            await PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return boxes;
        }
        public async Task<List<SelectBoxDTO>> GetUserBoxesList(BoxPaginationDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateBoxType(body.TypeID);

            var boxes = GetPaginationListOfObjects(body.Page, body.PageSize, body.TypeID, userID, _box.SelectUserBoxes);

            await PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return boxes;
        }



        public int GetDefaultBoxesTotalPages(int pageSize)
        {
            var totalPages = GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _box.SelectCountOfBoxes);
            return totalPages;
        }

        public int GetUserBoxesTotalPages(Guid userID, int pageSize)
        {
            ValidateUserID(userID);

            var totalPages = GetPaginationTotalPages(pageSize, userID, _box.SelectCountOfBoxes);
            return totalPages;
        }



        public List<SelectBoxTypeDTO> GetBoxTypes()
        {
            var types = _box.SelectBoxTypes();
            return types;
        }



        private void ValidateBoxID(Guid boxID)
        {
            if (!_box.IsBoxExist(boxID))
                throw new BadRequestException(_localization.FileIsNotExist());
        }

        private void ValidateBoxOwner(Guid boxID, Guid userID)
        {
            if (!_box.IsBoxOwner(boxID, userID))
                throw new BadRequestException(_localization.UserIsNotBoxOwner());
        }

        private void ValidateBoxType(Guid boxTypeID)
        {
            if (!_box.IsBoxTypeExist(boxTypeID))
                throw new BadRequestException(_localization.BoxTypeIsNotExist());
        }

        private void ValidateBoxTitle(Guid userID, string? title)
        {
            _rgx.ValidateBoxTitle(title ?? "");
            if (_box.IsBoxTitleBusy(userID, title!))
                throw new BadRequestException(_localization.BoxTitleIsBusy());
        }
    }
}