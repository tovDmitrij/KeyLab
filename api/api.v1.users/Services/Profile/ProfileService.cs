using component.v1.exceptions;

using db.v1.users.Repositories.User;
using helper.v1.localization.Helper.Interfaces;

namespace api.v1.users.Services.Profile
{
    public sealed class ProfileService(IUserRepository user, IProfileLocalizationHelper localization) : IProfileService
    {
        private readonly IUserRepository _user = user;
        private readonly IProfileLocalizationHelper _localization = localization;

        public string GetUserNickname(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());

            var nickname = _user.SelectUserNickname(userID)!;
            return nickname;
        }
    }
}