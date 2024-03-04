using api.v1.main.DTOs.User;

using component.v1.exceptions;

namespace api.v1.main.Services.User
{
    public interface IUserService
    {
        /// <exception cref="BadRequestException"></exception>
        public void SignUp(PostSignUpDTO body);

        /// <exception cref="BadRequestException"></exception>
        public JWTTokensDTO SignIn(PostSignInDTO body);

        /// <exception cref="BadRequestException"></exception>
        public string UpdateAccessToken(string refreshToken);
    }
}