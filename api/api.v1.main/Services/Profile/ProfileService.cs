using component.v1.exceptions;

using db.v1.main.Repositories.User;

using helper.v1.localization.Helper;

namespace api.v1.main.Services.Profile
{
    public sealed class ProfileService(IUserRepository user, ILocalizationHelper localization) : IProfileService
    {
        private readonly IUserRepository _user = user;
        private readonly ILocalizationHelper _localization = localization;

        public string GetUserNickname(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());

            var nickname = _user.SelectUserNickname(userID)!;
            return nickname;
        }
    }
}