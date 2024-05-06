using api.v1.keyboards.DTOs.Box;

using component.v1.exceptions;

using db.v1.keyboards.DTOs;
using db.v1.keyboards.Repositories.Box;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.regex.Interfaces;
using helper.v1.time;
using helper.v1.localization.Helper;
using helper.v1.file;
using helper.v1.messageBroker;
using helper.v1.localization.Helper.Interfaces;

namespace api.v1.keyboards.Services.Box
{
    public sealed class BoxService(IBoxRepository box, IUserRepository user, ICacheHelper cache,
        IFileConfigurationHelper fileCfg, IFileHelper file, IBoxRegexHelper rgx, ITimeHelper time, 
        IFileLocalizationHelper localization, IActivityConfigurationHelper activityCfg, IMessageBrokerHelper broker, 
        ICacheConfigurationHelper cacheCfg) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IBoxService
    {
        private readonly IBoxRepository _box = box;
        private readonly IBoxRegexHelper _rgx = rgx;
        private readonly ITimeHelper _time = time;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;



        public async Task AddBox(IFormFile? file, IFormFile? preview, string? title, Guid typeID, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateBoxType(typeID);
            ValidateBoxTitle(userID, title);

            var fileName = UploadFile(file, preview, userID,  _fileCfg.GetModelFilenameExtension, 
                _fileCfg.GetPreviewFilenameExtension, _fileCfg.GetBoxFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            _box.InsertBox(userID, typeID, title!, fileName, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task UpdateBox(IFormFile? file, IFormFile? preview, string? title, Guid userID, Guid boxID, Guid statsID)
        {
            ValidateBoxTitle(userID, title);
            ValidateBoxOwner(boxID, userID);

            UpdateFile(file, preview, userID, boxID, _box.SelectBoxFileName!,
                _fileCfg.GetModelFilenameExtension, _fileCfg.GetPreviewFilenameExtension, _fileCfg.GetBoxFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            _box.UpdateBoxTitle(boxID, title!, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task PatchBoxTitle(PatchBoxTitleDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateBoxID(body.BoxID);
            ValidateBoxTitle(userID, body.Title);
            ValidateBoxOwner(body.BoxID, userID);

            var currentTime = _time.GetCurrentUNIXTime();
            _box.UpdateBoxTitle(body.BoxID, body.Title, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task DeleteBox(DeleteBoxDTO body, Guid userID, Guid statsID)
        {
            ValidateBoxOwner(body.BoxID, userID);

            var fileName = _box.SelectBoxFileName(body.BoxID)!;
            var fileExtension = _fileCfg.GetModelFilenameExtension();
            var filePath = _fileCfg.GetBoxFilePath(userID, fileName, fileExtension);

            var previewExtension = _fileCfg.GetPreviewFilenameExtension();
            var previewPath = _fileCfg.GetBoxFilePath(userID, fileName, previewExtension);

            _file.DeleteFile(filePath);
            _file.DeleteFile(previewPath);

            _cache.DeleteValue(filePath);
            _cache.DeleteValue(previewPath);

            _box.DeleteBox(body.BoxID);

            await PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }



        public async Task<byte[]> GetBoxFileBytes(Guid boxID, Guid statsID)
        {
            var file = await GetFile(boxID, _fileCfg.GetModelFilenameExtension);

            await PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return file;
        }

        public async Task<string> GetBoxBase64Preview(Guid boxID)
        {
            var preview = await GetFile(boxID, _fileCfg.GetPreviewFilenameExtension);
            return Convert.ToBase64String(preview);
        }

        private async Task<byte[]> GetFile(Guid boxID, Func<string> cfgFuncFileExtension)
        {
            ValidateBoxID(boxID);

            var userID = _box.SelectBoxOwnerID(boxID);
            var fileName = _box.SelectBoxFileName(boxID);
            var fileExtension = cfgFuncFileExtension();
            var filePath = _fileCfg.GetBoxFilePath((Guid)userID!, fileName!, fileExtension);

            var file = await ReadFile(filePath);
            return file;
        }



        public async Task<List<SelectBoxDTO>> GetDefaultBoxesList(int page, int pageSize, Guid boxTypeID, Guid statsID)
        {
            ValidateBoxType(boxTypeID);
            var userID = _fileCfg.GetDefaultModelsUserID();

            var boxes = GetPaginationListOfObjects(page, pageSize, boxTypeID, userID, _box.SelectUserBoxes);

            await PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return boxes;
        }
        public async Task<List<SelectBoxDTO>> GetUserBoxesList(int page, int pageSize, Guid boxTypeID, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateBoxType(boxTypeID);

            var boxes = GetPaginationListOfObjects(page, pageSize, boxTypeID, userID, _box.SelectUserBoxes);

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