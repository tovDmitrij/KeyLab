using component.v1.exceptions;

using db.v1.main.Repositories.User;

using helper.v1.localization.Helper;

namespace api.v1.main.Services.Base
{
    public sealed class BaseService : IBaseService
    {
        private readonly ILocalizationHelper _localization;
        private readonly IUserRepository _user;

        public BaseService(ILocalizationHelper localization, IUserRepository user)
        {
            _localization = localization;
            _user = user;
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