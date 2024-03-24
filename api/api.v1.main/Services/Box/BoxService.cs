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
using api.v1.main.Services.Base;

namespace api.v1.main.Services.Box
{
    public sealed class BoxService : IBoxService
    {
        private readonly IBoxRepository _box;
        private readonly IUserRepository _user;

        private readonly ICacheHelper _cache;
        private readonly IFileConfigurationHelper _fileCfg;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly IPreviewConfigurationHelper _previewCfg;
        private readonly IFileHelper _file;
        private readonly IBoxRegexHelper _rgx;
        private readonly ITimeHelper _time;
        private readonly ILocalizationHelper _localization;
        private readonly IBaseService _base;

        public BoxService(IBoxRepository boxes, IUserRepository users, ICacheHelper cache,
                          IFileConfigurationHelper fileCfg, IFileHelper file, IBoxRegexHelper rgx,
                          ITimeHelper time, ICacheConfigurationHelper cacheCfg, ILocalizationHelper localization, 
                          IPreviewConfigurationHelper previewCfg, IBaseService @base)
        {
            _box = boxes;
            _user = users;
            _cache = cache;
            _fileCfg = fileCfg;
            _file = file;
            _rgx = rgx;
            _time = time;
            _cacheCfg = cacheCfg;
            _localization = localization;
            _previewCfg = previewCfg;
            _file = file;
            _base = @base;
        }



        public List<SelectBoxTypeDTO> GetBoxTypes()
        {
            var types = _box.SelectBoxTypes();
            return types;
        }



        public void AddBox(PostBoxDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateBoxFile(body.File);
            ValidateBoxPreview(body.Preview);
            ValidateBoxType(body.TypeID);
            ValidateBoxTitle(body.UserID, body.Title);

            var modelFileName = $"{body.Title}.glb";
            var modelFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, modelFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.File!.CopyTo(memoryStream);
                var modelBytes = memoryStream.ToArray();
                _file.AddFile(modelBytes, modelFilePath);
            }

            var fileType = _previewCfg.GetPreviewFileType();
            var imgFileName = $"{body.Title}.{fileType}";
            var imgFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, imgFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.Preview!.CopyTo(memoryStream);
                var imgBytes = memoryStream.ToArray();
                _file.AddFile(imgBytes, imgFilePath);
            }

            var currentTime = _time.GetCurrentUNIXTime();
            var insertBoxBody = new InsertBoxDTO(body.UserID, body.TypeID, body.Title!, modelFileName, imgFileName, currentTime);
            _box.InsertBoxInfo(insertBoxBody);
        }

        public void UpdateBox(PutBoxDTO body)
        {
            ValidateBoxExist(body.BoxID);
            ValidateUserID(body.UserID);
            ValidateBoxFile(body.File);
            ValidateBoxPreview(body.Preview);
            ValidateBoxTitle(body.UserID, body.Title);
            ValidateBoxOwner(body.BoxID, body.UserID);

            var oldModelFileName = _box.SelectBoxFileName(body.BoxID)!;
            var oldModelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, oldModelFileName);
            var newModelFileName = $"{body.Title}.glb";
            var newModelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, newModelFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.File!.CopyTo(memoryStream);
                var modelBytes = memoryStream.ToArray();
                _file.DeleteFile(oldModelFilePath);
                _file.AddFile(modelBytes, newModelFilePath);
            }

            var oldImgFileName = _box.SelectBoxPreviewName(body.BoxID)!;
            var oldImgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, oldImgFileName);
            var fileType = _previewCfg.GetPreviewFileType();
            var newImgFileName = $"{body.Title}.{fileType}";
            var newImgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, newImgFileName);
            using (var memoryStream = new MemoryStream())
            {
                body.Preview!.CopyTo(memoryStream);
                var imgBytes = memoryStream.ToArray();
                _file.DeleteFile(oldImgFilePath);
                _file.AddFile(imgBytes, newImgFilePath);
            }

            _cache.DeleteValue(body.BoxID);

            var updateBoxBody = new UpdateBoxDTO(body.BoxID, body.Title!, newModelFileName, newImgFileName);
            _box.UpdateBoxInfo(updateBoxBody);
        }

        public void DeleteBox(DeleteBoxDTO body, Guid userID)
        {
            ValidateBoxExist(body.BoxID);
            ValidateUserID(userID);
            ValidateBoxOwner(body.BoxID, userID);

            var modelFileName = _box.SelectBoxFileName(body.BoxID)!;
            var modelFilePath = _fileCfg.GetBoxModelFilePath(userID, modelFileName);

            var imgFileName = _box.SelectBoxPreviewName(body.BoxID)!;
            var imgFilePath = _fileCfg.GetBoxModelFilePath(userID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _cache.DeleteValue(body.BoxID);
            _box.DeleteBoxInfo(body.BoxID);
        }

        public byte[] GetBoxFile(Guid boxID)
        {
            var fileName = _box.SelectBoxFileName(boxID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _box.SelectBoxOwnerID(boxID) ?? throw new BadRequestException(_localization.FileIsNotExist());

            var filePath = _fileCfg.GetBoxModelFilePath(userID, fileName);

            if (!_cache.TryGetValue(boxID, out byte[]? file))
            {
                file = _file.GetFile(filePath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var cacheExpireTime = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(boxID, file, cacheExpireTime);
            }

            return file!;
        }

        public string GetBoxPreview(Guid keyboardID)
        {
            var previewName = _box.SelectBoxPreviewName(keyboardID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _box.SelectBoxOwnerID(keyboardID) ?? throw new BadRequestException(_localization.FileIsNotExist());

            var filePath = _fileCfg.GetBoxModelFilePath(userID, previewName);

            if (!_cache.TryGetValue(filePath, out byte[]? preview))
            {
                try
                {
                    preview = _file.GetFile(filePath);
                }
                catch
                {
                    var errorImgPath = _fileCfg.GetErrorImageFilePath();
                    preview = _file.GetFile(errorImgPath);
                }

                if (preview.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(filePath, preview, minutes);
            }

            return Convert.ToBase64String(preview!);
        }



        public List<SelectBoxDTO> GetDefaultBoxesList(BoxPaginationDTO body) => GetBoxesList(body, _fileCfg.GetDefaultModelsUserID());
        public List<SelectBoxDTO> GetUserBoxesList(BoxPaginationDTO body, Guid userID) => GetBoxesList(body, userID);

        public int GetDefaultBoxesTotalPages(int pageSize) => 
            _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _box.SelectCountOfBoxes);
        public int GetUserBoxesTotalPages(Guid userID, int pageSize) => 
            _base.GetPaginationTotalPages(pageSize, userID, _box.SelectCountOfBoxes);



        private List<SelectBoxDTO> GetBoxesList(BoxPaginationDTO body, Guid userID)
        {
            ValidateUserID(userID);
            ValidateBoxType(body.TypeID);
            ValidatePageSize(body.PageSize);
            ValidatePage(body.Page);

            var boxes = _box.SelectUserBoxes(body.Page, body.PageSize, body.TypeID, userID);
            return boxes;
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

        private void ValidatePageSize(int pageSize)
        {
            if (pageSize < 1)
                throw new BadRequestException(_localization.PaginationPageSizeIsNotValid());
        }

        private void ValidatePage(int page)
        {
            if (page < 1)
                throw new BadRequestException(_localization.PaginationPageIsNotValid());
        }
    }
}