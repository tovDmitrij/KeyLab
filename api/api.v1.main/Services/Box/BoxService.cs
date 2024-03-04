using api.v1.main.DTOs.Box;

using component.v1.exceptions;

using db.v1.main.DTOs.Box;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file.File;
using helper.v1.regex.Interfaces;
using helper.v1.time;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Box
{
    public sealed class BoxService : IBoxService
    {
        private readonly IBoxRepository _box;
        private readonly IUserRepository _user;

        private readonly ICacheHelper _cache;
        private readonly IFileConfigurationHelper _fileCfg;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly IFileHelper _file;
        private readonly IBoxRegexHelper _rgx;
        private readonly ITimeHelper _time;
        private readonly ILocalizationHelper _localization;

        public BoxService(IBoxRepository boxes, IUserRepository users, ICacheHelper cache,
                          IFileConfigurationHelper fileCfg, IFileHelper file, IBoxRegexHelper rgx,
                          ITimeHelper time, ICacheConfigurationHelper cacheCfg, ILocalizationHelper localization)
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
        }

        public void AddBox(PostBoxDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateBoxFile(body.File);
            ValidateBoxType(body.TypeID);
            ValidateBoxTitle(body.UserID, body.Title);
            ValidateBoxDescription(body.Description);

            var currentTime = _time.GetCurrentUNIXTime();

            Guid boxID = default;
            try
            {
                var filePath = $"{body.UserID}/boxes/{body.Title}.glb";
                var insertBoxBody = new InsertBoxDTO(body.UserID, body.TypeID, body.Title, body.Description, filePath, currentTime);
                _box.InsertBoxInfo(insertBoxBody);

                using var memoryStream = new MemoryStream();
                body.File!.CopyTo(memoryStream);
                var bytes = memoryStream.ToArray();

                var parentDirectory = _fileCfg.GetModelsParentDirectory();
                var fullPath = Path.Combine(parentDirectory, filePath);
                _file.AddFile(bytes, fullPath);

                _cache.DeleteValue($"{body.UserID}/boxes");
            }
            catch
            {
                _box.DeleteBoxInfo(boxID);
                throw;
            }
        }

        public void UpdateBox(PutBoxDTO body)
        {
            throw new NotImplementedException();
        }

        public void DeleteBox(DeleteBoxDTO body)
        {
            throw new NotImplementedException();
        }

        public byte[] GetBoxFile(Guid boxID)
        {
            var boxPath = _box.SelectBoxFilePath(boxID) ??
                throw new BadRequestException(_localization.FileIsNotExist());

            var parentDirectory = _fileCfg.GetModelsParentDirectory();
            var fullpath = Path.Combine(parentDirectory, boxPath);

            if (!_cache.TryGetValue(boxID, out byte[]? file))
            {
                file = _file.GetFile(fullpath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var cacheExpireTime = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(boxID, file, cacheExpireTime);
            }

            return file!;
        }



        public List<SelectBoxDTO> GetDefaultBoxesList() => GetBoxesList(_fileCfg.GetDefaultModelsUserID());

        public List<SelectBoxDTO> GetUserBoxesList(Guid userID) => GetBoxesList(userID);

       

        private List<SelectBoxDTO> GetBoxesList(Guid userID)
        {
            ValidateUserID(userID);

            if (!_cache.TryGetValue($"{userID}/boxes", out List<SelectBoxDTO>? boxes))
            {
                boxes = _box.SelectUserBoxes(userID);

                var cacheExpireTime = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue($"{userID}/boxes", boxes, cacheExpireTime);
            }
            return boxes!;
        }



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
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

        private void ValidateBoxDescription(string? description)
        {
            if (description != null)
                _rgx.ValidateBoxDescription(description);
        }
    }
}