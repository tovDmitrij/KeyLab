using api.v1.main.DTOs;

using component.v1.activity;
using component.v1.exceptions;

using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;

namespace api.v1.main.Services.BaseAlgorithm
{
    public sealed class BaseAlgorithmService(ILocalizationHelper localization, IUserRepository user, ICacheHelper cache, 
        ICacheConfigurationHelper cacheCfg, IFileHelper file, IMessageBrokerHelper broker) : IBaseAlgorithmService
    {
        private readonly ILocalizationHelper _localization = localization;
        private readonly IUserRepository _user = user;
        private readonly ICacheHelper _cache = cache;
        private readonly IFileHelper _file = file;
        private readonly IMessageBrokerHelper _broker = broker;
        private readonly ICacheConfigurationHelper _cacheCfg = cacheCfg;

        public byte[] GetFile(string filePath)
        {
            if (!_cache.TryGetValue(filePath, out byte[]? file))
            {
                file = _file.GetFile(filePath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(filePath, file, minutes);
            }
            return file!;
        }



        public InitFileDTO AddFile(
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
                _file.AddFile(modelBytes, filePath);
            }

            var previewName = $"{title}.jpeg";
            var previewPath = filePathFunction(userID, previewName);
            using (var ms = new MemoryStream())
            {
                preview!.CopyTo(ms);
                var imgBytes = ms.ToArray();
                _file.AddFile(imgBytes, previewPath);
            }

            return new(fileName, previewName);
        }

        public InitFileDTO AddFile(
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
                _file.AddFile(modelBytes, filePath);
            }

            var previewName = $"{title}.jpeg";
            var previewPath = filePathFunction(userID, objectID, previewName);
            using (var ms = new MemoryStream())
            {
                preview!.CopyTo(ms);
                var imgBytes = ms.ToArray();
                _file.AddFile(imgBytes, previewPath);
            }

            return new(fileName, previewName);
        }

        public InitFileDTO UpdateFile(
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
                _file.AddFile(modelBytes, newFilePath);
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
                _file.AddFile(imgBytes, newPreviewPath);
            }

            _cache.DeleteValue(objectID);

            return new(newFileName, newPreviewName);
        }



        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Func<int, int, List<Object>> repositoryFunction)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize);
            return objects;
        }

        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid param1,
            Func<int, int, Guid, List<Object>> repositoryFunction)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize, param1);
            return objects;
        }

        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid param1,
            Guid param2,
            Func<int, int, Guid, Guid, List<Object>> repositoryFunction)
        {
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize, param1, param2);
            return objects;
        }



        public int GetPaginationTotalPages(int pageSize, Func<int> repositoryFunction)
        {
            ValidatePageSize(pageSize);

            var count = repositoryFunction();
            return GetTotalPages(count, pageSize);
        }

        public int GetPaginationTotalPages(int pageSize, Guid param, Func<Guid, int> repositoryFunction)
        {
            ValidatePageSize(pageSize);

            var count = repositoryFunction(param);
            return GetTotalPages(count, pageSize);
        }



        public async Task PublishActivity(Guid statsID, Func<string> activityTagFunction)
        {
            var activityTag = activityTagFunction();
            var activityBody = new ActivityDTO(statsID, activityTag);
            await _broker.PublishData(activityBody);
        }



        private static int GetTotalPages(int count, int pageSize) => (int)Math.Ceiling((double)count / pageSize);


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

        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
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