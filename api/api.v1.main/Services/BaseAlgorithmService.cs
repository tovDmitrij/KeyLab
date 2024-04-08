using api.v1.main.DTOs;
using component.v1.activity;
using component.v1.exceptions;

using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;

namespace api.v1.main.Services
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


        protected InitFileDTO UploadFile(
            IFormFile? file,
            IFormFile? preview,
            Guid userID,
            string title,
            Func<Guid, string, string> filePathFunction)
        {
            ValidateUserID(userID);
            ValidateFile(file);
            ValidatePreview(preview);

            var fileName = $"{title}.glb";
            var filePath = filePathFunction(userID, fileName);
            using (var ms = new MemoryStream())
            {
                file!.CopyTo(ms);
                var modelBytes = ms.ToArray();
                _file.UploadFileAsync(modelBytes, filePath);
            }

            var previewName = $"{title}.jpeg";
            var previewPath = filePathFunction(userID, previewName);
            using (var ms = new MemoryStream())
            {
                preview!.CopyTo(ms);
                var imgBytes = ms.ToArray();
                _file.UploadFileAsync(imgBytes, previewPath);
            }

            return new(fileName, previewName);
        }

        protected InitFileDTO UploadFile(
            IFormFile? file,
            IFormFile? preview,
            Guid userID,
            Guid objectID,
            string title,
            Func<Guid, Guid, string, string> filePathFunction)
        {
            ValidateUserID(userID);
            ValidateFile(file);
            ValidatePreview(preview);

            var fileName = $"{title}.glb";
            var filePath = filePathFunction(userID, objectID, fileName);
            using (var ms = new MemoryStream())
            {
                file!.CopyTo(ms);
                var modelBytes = ms.ToArray();
                _file.UploadFileAsync(modelBytes, filePath);
            }

            var previewName = $"{title}.jpeg";
            var previewPath = filePathFunction(userID, objectID, previewName);
            using (var ms = new MemoryStream())
            {
                preview!.CopyTo(ms);
                var imgBytes = ms.ToArray();
                _file.UploadFileAsync(imgBytes, previewPath);
            }

            return new(fileName, previewName);
        }

        protected InitFileDTO UpdateFile(
            IFormFile? file,
            IFormFile? preview,
            Guid userID,
            Guid objectID,
            string title,
            Func<Guid, string> fileNameFunction,
            Func<Guid, string> previewNameFunction,
            Func<Guid, string, string> filePathFunction)
        {
            ValidateUserID(userID);
            ValidateFile(file);
            ValidatePreview(preview);

            var oldFileName = fileNameFunction(objectID);
            var oldFilePath = filePathFunction(userID, oldFileName);
            var newFileName = $"{title}.glb";
            var newFilePath = filePathFunction(userID, newFileName);
            using (var ms = new MemoryStream())
            {
                file!.CopyTo(ms);
                var modelBytes = ms.ToArray();
                _file.DeleteFile(oldFilePath);
                _file.UploadFileAsync(modelBytes, newFilePath);
            }

            var oldPreviewFileName = previewNameFunction(objectID)!;
            var oldPreviewFilePath = filePathFunction(userID, oldPreviewFileName);
            var newPreviewName = $"{title}.jpeg";
            var newPreviewPath = filePathFunction(userID, newPreviewName);
            using (var memoryStream = new MemoryStream())
            {
                preview!.CopyTo(memoryStream);
                var imgBytes = memoryStream.ToArray();
                _file.DeleteFile(oldPreviewFilePath);
                _file.UploadFileAsync(imgBytes, newPreviewPath);
            }

            var cacheKey = _cacheCfg.GetFileCacheKey(oldFilePath);
            _cache.DeleteValue(cacheKey);

            return new(newFileName, newPreviewName);
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


        protected List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Func<int, int, List<Object>> repositoryFunction)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize);
            /*
            var cacheKey = _cacheCfg.GetPaginationListCacheKey(page, pageSize, repositoryFunction.GetHashCode());
            if (!_cache.TryGetValue(cacheKey, out List<Object>? objects))
            {
                objects = repositoryFunction(page, pageSize);

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, objects, minutes);
            }
            */
            return objects!;
        }

        protected List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid param1,
            Func<int, int, Guid, List<Object>> repositoryFunction)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize, param1);
            /*
            var cacheKey = _cacheCfg.GetPaginationListCacheKey(page, pageSize, repositoryFunction.GetHashCode(), param1);
            if (!_cache.TryGetValue(cacheKey, out List<Object>? objects))
            {
                objects = repositoryFunction(page, pageSize, param1);

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, objects, minutes);
            }
            */
            return objects!;
        }

        protected List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid param1,
            Guid param2,
            Func<int, int, Guid, Guid, List<Object>> repositoryFunction)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize, param1, param2);
            /*
            var cacheKey = _cacheCfg.GetPaginationListCacheKey(page, pageSize, repositoryFunction.GetHashCode(), param1, param2);
            if (!_cache.TryGetValue(cacheKey, out List<Object>? objects))
            {
                objects = repositoryFunction(page, pageSize, param1, param2);

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, objects, minutes);
            }
             */
            return objects!;
        }



        protected int GetPaginationTotalPages(int pageSize, Func<int> repositoryFunction)
        {
            ValidatePageSize(pageSize);

            var count = repositoryFunction();
            return GetTotalPages(count, pageSize);
        }

        protected int GetPaginationTotalPages(int pageSize, Guid param, Func<Guid, int> repositoryFunction)
        {
            ValidatePageSize(pageSize);

            var count = repositoryFunction(param);
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