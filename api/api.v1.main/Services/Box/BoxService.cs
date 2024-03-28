using api.v1.main.DTOs.Box;

using component.v1.exceptions;

using db.v1.main.DTOs.Box;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.regex.Interfaces;
using helper.v1.time;
using helper.v1.localization.Helper;
using helper.v1.file;
using db.v1.main.DTOs.BoxType;
using api.v1.main.Services.BaseAlgorithm;
using MassTransit.Courier.Contracts;

namespace api.v1.main.Services.Box
{
    public sealed class BoxService(IBoxRepository box, IUserRepository user, ICacheHelper cache,
        IFileConfigurationHelper fileCfg, IFileHelper file, IBoxRegexHelper rgx, ITimeHelper time, ILocalizationHelper localization, 
        IBaseAlgorithmService @base, IActivityConfigurationHelper activityCfg) : IBoxService
    {
        private readonly IBoxRepository _box = box;
        private readonly IUserRepository _user = user;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly ICacheHelper _cache = cache;
        private readonly IFileHelper _file = file;
        private readonly IBoxRegexHelper _rgx = rgx;
        private readonly ITimeHelper _time = time;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;
        private readonly ILocalizationHelper _localization = localization;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;

        public async Task AddBox(PostBoxDTO body, Guid statsID)
        {
            ValidateUserID(body.UserID);
            ValidateBoxFile(body.File);
            ValidateBoxPreview(body.Preview);
            ValidateBoxType(body.TypeID);
            ValidateBoxTitle(body.UserID, body.Title);

            var names = _base.AddFile(body.File, body.Preview, body.UserID, body.Title!, _fileCfg.GetBoxFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertBoxBody = new InsertBoxDTO(body.UserID, body.TypeID, body.Title!, names.FileName, names.PreviewName, currentTime);
            _box.InsertBoxInfo(insertBoxBody);

            await _base.PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task UpdateBox(PutBoxDTO body, Guid statsID)
        {
            ValidateBoxExist(body.BoxID);
            ValidateBoxTitle(body.UserID, body.Title);
            ValidateBoxOwner(body.BoxID, body.UserID);

            var names = _base.UpdateFile(body.File, body.Preview, body.UserID, body.BoxID, body.Title!, _box.SelectBoxFileName, _box.SelectBoxPreviewName, _fileCfg.GetBoxFilePath);

            var updateBoxBody = new UpdateBoxDTO(body.BoxID, body.Title!, names.FileName, names.PreviewName);
            _box.UpdateBoxInfo(updateBoxBody);

            await _base.PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }

        public async Task DeleteBox(DeleteBoxDTO body, Guid userID, Guid statsID)
        {
            ValidateBoxExist(body.BoxID);
            ValidateUserID(userID);
            ValidateBoxOwner(body.BoxID, userID);

            var modelFileName = _box.SelectBoxFileName(body.BoxID)!;
            var modelFilePath = _fileCfg.GetBoxFilePath(userID, modelFileName);

            var imgFileName = _box.SelectBoxPreviewName(body.BoxID)!;
            var imgFilePath = _fileCfg.GetBoxFilePath(userID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _cache.DeleteValue(body.BoxID);
            _box.DeleteBoxInfo(body.BoxID);

            await _base.PublishActivity(statsID, _activityCfg.GetEditBoxActivityTag);
        }



        public async Task<byte[]> GetBoxFileBytes(Guid boxID, Guid statsID)
        {
            ValidateBoxID(boxID);
            var fileName = _box.SelectBoxFileName(boxID);
            var userID = _box.SelectBoxOwnerID(boxID);
            var filePath = _fileCfg.GetBoxFilePath((Guid)userID!, fileName!);

            var file = _base.GetFile(filePath);

            await _base.PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return file;
        }

        public string GetBoxBase64Preview(Guid boxID)
        {
            ValidateBoxID(boxID);
            var previewName = _box.SelectBoxPreviewName(boxID);
            var userID = _box.SelectBoxOwnerID(boxID);
            var previewPath = _fileCfg.GetBoxFilePath((Guid)userID!, previewName!);

            var preview = _base.GetFile(previewPath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectBoxDTO>> GetDefaultBoxesList(BoxPaginationDTO body, Guid statsID)
        {
            ValidateBoxType(body.TypeID);
            var userID = _fileCfg.GetDefaultModelsUserID();

            var boxes = _base.GetPaginationListOfObjects(body.Page, body.PageSize, body.TypeID, userID, _box.SelectUserBoxes);

            await _base.PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return boxes;
        }
        public async Task<List<SelectBoxDTO>> GetUserBoxesList(BoxPaginationDTO body, Guid userID, Guid statsID)
        {
            ValidateBoxType(body.TypeID);
            ValidateUserID(userID);

            var boxes = _base.GetPaginationListOfObjects(body.Page, body.PageSize, body.TypeID, userID, _box.SelectUserBoxes);

            await _base.PublishActivity(statsID, _activityCfg.GetSeeBoxActivityTag);
            return boxes;
        }



        public int GetDefaultBoxesTotalPages(int pageSize)
        {
            var totalPages = _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _box.SelectCountOfBoxes);
            return totalPages;
        }

        public int GetUserBoxesTotalPages(Guid userID, int pageSize)
        {
            ValidateUserID(userID);

            var totalPages = _base.GetPaginationTotalPages(pageSize, userID, _box.SelectCountOfBoxes);
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

        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }

        private void ValidateBoxOwner(Guid boxID, Guid userID)
        {
            if (!_box.IsBoxOwner(boxID, userID))
                throw new BadRequestException(_localization.UserIsNotBoxOwner());
        }

        private void ValidateBoxFile(IFormFile? file)
        {
            if (file is null || file.Length == 0)
                throw new BadRequestException(_localization.FileIsNotAttached());
        }

        private void ValidateBoxPreview(IFormFile? preview)
        {
            if (preview == null || preview.Length == 0)
                throw new BadRequestException(_localization.PreviewIsNotAttached());
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

        private void ValidateBoxExist(Guid boxID)
        {
            if (!_box.IsBoxExist(boxID))
                throw new BadRequestException(_localization.FileIsNotExist());
        }
    }
}