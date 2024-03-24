using api.v1.main.DTOs;

using component.v1.exceptions;

using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Base
{
    public sealed class BaseService : IBaseService
    {
        private readonly ILocalizationHelper _localization;
        private readonly IUserRepository _user;
        private readonly ICacheHelper _cache;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly IFileHelper _file;

        public BaseService(ILocalizationHelper localization, IUserRepository user, ICacheHelper cache, ICacheConfigurationHelper cacheCfg, 
                           IFileHelper file)
        {
            _localization = localization;
            _user = user;
            _cache = cache;
            _cacheCfg = cacheCfg;
            _file = file;
        }



        public byte[] GetFile(
            Guid fileID,
            Func<Guid, string?> fileNameFunction,
            Func<string, string> filePathFunction)
        {
            var fileName = fileNameFunction(fileID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = filePathFunction(fileName);

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

        public byte[] GetFile(
            Guid fileID,
            Func<Guid, string?> fileNameFunction,
            Func<Guid, Guid?> userIDFunction,
            Func<Guid, string, string> filePathFunction)
        {
            var fileName = fileNameFunction(fileID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = userIDFunction(fileID) ?? throw new BadRequestException(_localization.FileIsNotExist());

            var filePath = filePathFunction(userID, fileName);

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
            Guid userID,
            Func<int, int, Guid, List<Object>> repositoryFunction)
        {
            ValidateUserID(userID);
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize, userID);
            return objects;
        }

        public List<Object> GetPaginationListOfObjects<Object>(
            int page,
            int pageSize,
            Guid userID,
            Guid objectTypeID,
            Func<int, int, Guid, Guid, List<Object>> repositoryFunction)
        {
            ValidateUserID(userID);
            ValidatePageSize(pageSize);
            ValidatePage(page);

            var objects = repositoryFunction(page, pageSize, objectTypeID, userID);
            return objects;
        }



        public int GetPaginationTotalPages(int pageSize, Func<int> repositoryFunction)
        {
            ValidatePageSize(pageSize);

            var count = repositoryFunction();
            return GetTotalPages(count, pageSize);
        }

        public int GetPaginationTotalPages(int pageSize, Guid userID, Func<Guid, int> repositoryFunction)
        {
            ValidatePageSize(pageSize);

            ValidateUserID(userID);
            var count = repositoryFunction(userID);
            return GetTotalPages(count, pageSize);
        }



        private static int GetTotalPages(int count, int pageSize) => (int)Math.Ceiling((double)count / pageSize);



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