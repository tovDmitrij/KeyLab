using api.v1.main.DTOs.User;

using component.v1.exceptions;

namespace api.v1.main.Services.User
{
    public interface IUserService
    {
        /// <exception cref="BadRequestException"></exception>
        public void SignUp(PostSignUpDTO body);

        /// <exception cref="BadRequestException"></exception>
        public Task<SignInDTO> SignIn(PostSignInDTO body, Guid statsID);

        /// <exception cref="BadRequestException"></exception>
        public Task<string> UpdateAccessToken(string refreshToken, Guid statsID);
    }
}