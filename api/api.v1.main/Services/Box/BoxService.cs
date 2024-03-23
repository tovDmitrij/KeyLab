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
using helper.v1.messageBroker;
using api.v1.main.DTOs;
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
            ValidateBoxType(body.TypeID);
            ValidateBoxTitle(body.UserID, body.Title);

            var modelFileName = $"{body.Title}.glb";
            var modelFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, modelFileName);

            using var memoryStream = new MemoryStream();
            body.File!.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();

            var fileType = _previewCfg.GetPreviewFileType();

            var imgFileName = $"{body.Title}.{fileType}";
            var imgFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, imgFileName);

            //var imgBase64 = Convert.ToBase64String(bytes);
            //var previewBody = new PreviewDTO(imgFilePath, imgBase64);
            //await _broker.SendData(previewBody);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertBoxBody = new InsertBoxDTO(body.UserID, body.TypeID, body.Title, modelFileName, imgFileName, currentTime);
            _box.InsertBoxInfo(insertBoxBody);

            _file.AddFile(bytes, modelFilePath);
        }

        public void UpdateBox(PutBoxDTO body)
        {
            ValidateBoxExist(body.BoxID);
            ValidateUserID(body.UserID);
            ValidateBoxFile(body.File);
            ValidateBoxTitle(body.UserID, body.Title);
            ValidateBoxOwner(body.BoxID, body.UserID);


            var oldModelFileName = _box.SelectBoxFileName(body.BoxID)!;
            var oldModelFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, oldModelFileName);

            var newModelFileName = $"{body.Title}.glb";
            var newModelFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, newModelFileName);


            using var memoryStream = new MemoryStream();
            body.File!.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();


            var oldImgFileName = _box.SelectBoxPreviewName(body.BoxID)!;
            var oldImgFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, oldImgFileName);

            var fileType = _previewCfg.GetPreviewFileType();

            var newImgFileName = $"{body.Title}.{fileType}";
            var newImgFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, newImgFileName);


            //var imgBase64 = Convert.ToBase64String(bytes);
            //var previewBody = new PreviewDTO(newImgFilePath, imgBase64);
            //_file.DeleteFile(oldImgFilePath);
            //await _broker.SendData(previewBody);


            var updateBoxBody = new UpdateBoxDTO(body.BoxID, body.Title, newModelFileName, newImgFileName);
            _box.UpdateBoxInfo(updateBoxBody);


            _file.UpdateFile(bytes, oldModelFilePath);
            _file.MoveFile(oldModelFilePath, newModelFilePath);

        }

        public void DeleteBox(DeleteBoxDTO body)
        {
            ValidateBoxExist(body.BoxID);
            ValidateUserID(body.UserID);
            ValidateBoxOwner(body.BoxID, body.UserID);

            var modelFileName = _box.SelectBoxFileName(body.BoxID)!;
            var modelFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, modelFileName);

            var imgFileName = _box.SelectBoxPreviewName(body.BoxID)!;
            var imgFilePath = _fileCfg.GetBoxModelFilePath(body.UserID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _box.DeleteBoxInfo(body.BoxID);
            _cache.DeleteValue(body.BoxID);
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



        public List<BoxListDTO> GetDefaultBoxesList(BoxPaginationDTO body) => GetBoxesList(body, _fileCfg.GetDefaultModelsUserID());
        public List<BoxListDTO> GetUserBoxesList(BoxPaginationDTO body, Guid userID) => GetBoxesList(body, userID);

        public int GetDefaultBoxesTotalPages(int pageSize) => 
            _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _box.SelectCountOfBoxes);
        public int GetUserBoxesTotalPages(Guid userID, int pageSize) => 
            _base.GetPaginationTotalPages(pageSize, userID, _box.SelectCountOfBoxes);



        private List<BoxListDTO> GetBoxesList(BoxPaginationDTO body, Guid userID)
        {
            ValidateUserID(userID);
            ValidateBoxType(body.TypeID);
            ValidatePageSize(body.PageSize);
            ValidatePage(body.Page);

            var cacheKey = body.GetHashCode() + userID.GetHashCode();
            if (!_cache.TryGetValue(cacheKey, out List<BoxListDTO>? boxes))
            {
                boxes = new();

                var fileType = _previewCfg.GetPreviewFileType();
                var dbBoxes = _box.SelectUserBoxes(body.Page, body.PageSize, body.TypeID, userID);
                foreach (var dbBox in dbBoxes)
                {
                    var filePath = _fileCfg.GetBoxModelFilePath(userID, dbBox.PreviewName);

                    byte[] bytes;
                    try
                    {
                        bytes = _file.GetFile(filePath);
                    }
                    catch
                    {
                        var errorImgPath = _fileCfg.GetErrorImageFilePath();
                        bytes = _file.GetFile(errorImgPath);
                    }
                    var img = $"data:image/{fileType};base64," + Convert.ToBase64String(bytes);

                    boxes.Add(new(dbBox.ID, dbBox.TypeID, dbBox.TypeTitle, dbBox.Title, img, dbBox.CreationDate));
                }

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, boxes, minutes);
            }

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

        private void ValidateBoxType(Guid boxTypeID)
        {
            if (!_box.IsBoxTypeExist(boxTypeID))
                throw new BadRequestException(_localization.BoxTypeIsNotExist());
        }

        private void ValidateBoxTitle(Guid userID, string title)
        {
            if (_box.IsBoxTitleBusy(userID, title))
                throw new BadRequestException(_localization.BoxTitleIsBusy());
            _rgx.ValidateBoxTitle(title);
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