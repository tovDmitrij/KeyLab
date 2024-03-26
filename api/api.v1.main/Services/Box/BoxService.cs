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

namespace api.v1.main.Services.Box
{
    public sealed class BoxService(IBoxRepository box, IUserRepository user, ICacheHelper cache,
                          IFileConfigurationHelper fileCfg, IFileHelper file, IBoxRegexHelper rgx,
                          ITimeHelper time, ILocalizationHelper localization, IBaseAlgorithmService @base) : IBoxService
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

        public void AddBox(PostBoxDTO body)
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
        }

        public void UpdateBox(PutBoxDTO body)
        {
            ValidateBoxExist(body.BoxID);
            ValidateBoxTitle(body.UserID, body.Title);
            ValidateBoxOwner(body.BoxID, body.UserID);

            var names = _base.UpdateFile(body.File, body.Preview, body.UserID, body.BoxID, body.Title!, _box.SelectBoxFileName, _box.SelectBoxPreviewName, _fileCfg.GetBoxFilePath);

            var updateBoxBody = new UpdateBoxDTO(body.BoxID, body.Title!, names.FileName, names.PreviewName);
            _box.UpdateBoxInfo(updateBoxBody);
        }

        public void DeleteBox(DeleteBoxDTO body, Guid userID)
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
        }



        public byte[] GetBoxFile(Guid boxID)
        {
            var file = _base.GetFile(boxID, _box.SelectBoxFileName, _box.SelectBoxOwnerID, _fileCfg.GetBoxFilePath);
            return file;
        }

        public string GetBoxPreview(Guid boxID)
        {
            var preview = _base.GetFile(boxID, _box.SelectBoxPreviewName, _box.SelectBoxOwnerID, _fileCfg.GetBoxFilePath);
            return Convert.ToBase64String(preview);
        }



        public List<SelectBoxDTO> GetDefaultBoxesList(BoxPaginationDTO body)
        {
            ValidateBoxType(body.TypeID);
            var userID = _fileCfg.GetDefaultModelsUserID();

            var boxes = _base.GetPaginationListOfObjects(body.Page, body.PageSize, body.TypeID, userID, _box.SelectUserBoxes);
            return boxes;
        }
        public List<SelectBoxDTO> GetUserBoxesList(BoxPaginationDTO body, Guid userID)
        {
            ValidateBoxType(body.TypeID);
            ValidateUserID(userID);

            var boxes = _base.GetPaginationListOfObjects(body.Page, body.PageSize, body.TypeID, userID, _box.SelectUserBoxes);
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