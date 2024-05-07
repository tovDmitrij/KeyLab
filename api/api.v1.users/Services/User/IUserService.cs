using api.v1.users.DTOs;
using component.v1.exceptions;

namespace api.v1.users.Services.User
{
    public interface IUserService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task SignUp(string email, string password, string repeatedPassword, string nickname, string emailCode);
        /// <exception cref="BadRequestException"></exception>
        public SignInDTO SignIn(string email, string password);
        /// <exception cref="BadRequestException"></exception>
        public string UpdateAccessToken(string refreshToken);
    }
}