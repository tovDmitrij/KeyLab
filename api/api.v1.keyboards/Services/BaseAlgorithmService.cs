using component.v1.activity;
using component.v1.exceptions;

using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;

namespace api.v1.keyboards.Services
{
    public abstract class BaseAlgorithmService(ILocalizationHelper localization, IUserRepository user, ICacheHelper cache,
        ICacheConfigurationHelper cacheCfg, IFileHelper file, IMessageBrokerHelper broker)
    {
        protected readonly ILocalizationHelper _localization = localization;
        protected readonly IUserRepository _user = user;
        protected readonly ICacheHelper _cache = cache;
        protected readonly IFileHelper _file = file;
        protected readonly IMessageBrokerHelper _broker = broker;
        protected readonly ICacheConfigurationHelper _cacheCfg = cacheCfg;



        protected string UploadFile(IFormFile? file, IFormFile? preview, Guid userID, Func<string> cfgFuncFileExtension,
            Func<string> cfgFuncPreviewExtension, Func<Guid, string, string, string> cfgFuncFilePath)
        {
            ValidateUserID(userID);
            ValidateFile(file);
            ValidatePreview(preview);

            var fileName = Guid.NewGuid().ToString();
            var fileExtension = cfgFuncFileExtension();
            var filePath = cfgFuncFilePath(userID, fileName, fileExtension);
            using (var ms = new MemoryStream())
            {
                file!.CopyTo(ms);
                var modelBytes = ms.ToArray();
                _file.UploadFileAsync(modelBytes, filePath);
            }

            var previewExtension = cfgFuncPreviewExtension();
            var previewPath = cfgFuncFilePath(userID, fileName, previewExtension);
            using (var ms = new MemoryStream())
            {
                preview!.CopyTo(ms);
                var imgBytes = ms.ToArray();
                _file.UploadFileAsync(imgBytes, previewPath);
            }

            return fileName;
        }

        protected void UpdateFile(IFormFile? file, IFormFile? preview, Guid userID, Guid objectID,
            Func<Guid, string> reposFuncFileName, Func<string> cfgFuncFileExtension, Func<string> cfgFuncPreviewExtension,
            Func<Guid, string, string, string> cfgFuncFilePath)
        {
            ValidateUserID(userID);
            ValidateFile(file);
            ValidatePreview(preview);

            var fileName = reposFuncFileName(objectID);
            var fileExtension = cfgFuncFileExtension();
            var filePath = cfgFuncFilePath(userID, fileName, fileExtension);
            using (var ms = new MemoryStream())
            {
                file!.CopyTo(ms);
                var bytes = ms.ToArray();
                _file.DeleteFile(filePath);
                _file.UploadFileAsync(bytes, filePath);
            }

            var previewExtension = cfgFuncPreviewExtension();
            var previewPath = cfgFuncFilePath(userID, fileName, previewExtension);
            using (var memoryStream = new MemoryStream())
            {
                preview!.CopyTo(memoryStream);
                var imgBytes = memoryStream.ToArray();
                _file.DeleteFile(previewPath);
                _file.UploadFileAsync(imgBytes, previewPath);
            }

            var fileCacheKey = _cacheCfg.GetFileCacheKey(filePath);
            _cache.DeleteValue(fileCacheKey);

            var previewCacheKey = _cacheCfg.GetFileCacheKey(previewPath);
            _cache.DeleteValue(previewCacheKey);
        }

        protected void UpdateFile(IFormFile? file, Guid userID, Guid childObjectID, Guid parentObjectID,
            Func<Guid, string> reposFuncFileName, Func<string> cfgFuncFileExtension,
            Func<Guid, Guid, string, string, string> cfgFuncFilePath)
        {
            ValidateUserID(userID);
            ValidateFile(file);

            var fileName = reposFuncFileName(childObjectID);
            var fileExtension = cfgFuncFileExtension();
            var filePath = cfgFuncFilePath(userID, parentObjectID, fileName!, fileExtension);
            using (var ms = new MemoryStream())
            {
                file!.CopyTo(ms);
                var bytes = ms.ToArray();
                _file.DeleteFile(filePath);
                _file.UploadFileAsync(bytes, filePath);
            }

            var cacheKey = _cacheCfg.GetFileCacheKey(filePath);
            _cache.DeleteValue(cacheKey);
        }

        protected async Task<byte[]> ReadFile(string filePath)
        {
            var cacheKey = _cacheCfg.GetFileCacheKey(filePath);
            if (!_cache.TryGetValue(cacheKey, out byte[]? file))
            {
                file = await _file.ReadFileAsync(filePath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, file, minutes);
            }
            return file!;
        }



        protected List<Object> GetPaginationListOfObjects<Object>(int page, int pageSize, Func<int, int, List<Object>> reposFunc)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = reposFunc(page, pageSize);
            return objects!;
        }

        protected List<Object> GetPaginationListOfObjects<Object>(int page, int pageSize, Guid param1, 
            Func<int, int, Guid, List<Object>> reposFunc)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = reposFunc(page, pageSize, param1);
            return objects!;
        }

        protected List<Object> GetPaginationListOfObjects<Object>(int page, int pageSize, Guid param1, Guid param2,
            Func<int, int, Guid, Guid, List<Object>> reposFunc)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = reposFunc(page, pageSize, param1, param2);
            return objects!;
        }



        protected int GetPaginationTotalPages(int pageSize, Func<int> reposFunc)
        {
            ValidatePageSize(pageSize);

            var count = reposFunc();
            return GetTotalPages(count, pageSize);
        }

        protected int GetPaginationTotalPages(int pageSize, Guid param, Func<Guid, int> reposFunc)
        {
            ValidatePageSize(pageSize);

            var count = reposFunc(param);
            return GetTotalPages(count, pageSize);
        }



        protected async Task PublishActivity(Guid statsID, Func<string> activityTagFunction)
        {
            var activityTag = activityTagFunction();
            var activityBody = new ActivityDTO(statsID, activityTag);
            await _broker.PublishData(activityBody);
        }



        private static int GetTotalPages(int count, int pageSize) => (int)Math.Ceiling((double)count / pageSize);



        protected void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new UnauthorizedException(_localization.UserAccessTokenIsExpired());
        }

        private void ValidateFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException(_localization.FileIsNotAttached());
        }

        private void ValidatePreview(IFormFile? preview)
        {
            if (preview == null || preview.Length == 0)
                throw new BadRequestException(_localization.PreviewIsNotAttached());
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