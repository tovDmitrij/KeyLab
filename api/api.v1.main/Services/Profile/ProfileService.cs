using component.v1.exceptions;

using db.v1.main.Repositories.User;

using helper.v1.localization.Helper;

namespace api.v1.main.Services.Profile
{
    public sealed class ProfileService : IProfileService
    {
        private readonly IUserRepository _user;
        private readonly ILocalizationHelper _localization;

        public ProfileService(IUserRepository user, ILocalizationHelper localization)
        {
            _user = user;
            _localization = localization;
        }



        public string GetUserNickname(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());

            var nickname = _user.SelectUserNickname(userID)!;
            return nickname;
        }
    }
}