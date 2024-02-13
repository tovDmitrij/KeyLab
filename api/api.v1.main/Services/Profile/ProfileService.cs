
using component.v1.exceptions;

using db.v1.main.Repositories.User;

namespace api.v1.main.Services.Profile
{
    public sealed class ProfileService : IProfileService
    {
        private readonly IUserRepository _users;

        public ProfileService(IUserRepository users)
        {
            _users = users;
        }



        public string GetUserNickname(Guid userID)
        {
            if (_users.IsUserExist(userID))
                throw new BadRequestException("Пользователя с заданным идентификатором не существует");

            var nickname = _users.GetUserNicknameByID(userID)!;
            return nickname;
        }
    }
}