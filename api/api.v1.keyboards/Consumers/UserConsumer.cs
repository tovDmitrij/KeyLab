using component.v1.user;

using db.v1.users.Repositories.User;

using MassTransit;

namespace api.v1.keyboards.Consumers
{
    public sealed class UserConsumer(IUserRepository user) : IConsumer<UserDTO>
    {
        private readonly IUserRepository _user = user;

        public async Task Consume(ConsumeContext<UserDTO> context)
        {
            var msg = context.Message;

            _user.InsertUserInfo(new(msg.Email, msg.Salt, msg.HashPass, msg.Nickname, msg.RegistrationDate), msg.UserID);
        }
    }
}