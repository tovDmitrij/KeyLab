using api.v1.users.DTOs;
using component.v1.exceptions;

namespace api.v1.users.Services.User
{
    public interface IUserService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task SignUp(PostSignUpDTO body);
        /// <exception cref="BadRequestException"></exception>
        public SignInDTO SignIn(PostSignInDTO body);
        /// <exception cref="BadRequestException"></exception>
        public string UpdateAccessToken(string refreshToken);
    }
}