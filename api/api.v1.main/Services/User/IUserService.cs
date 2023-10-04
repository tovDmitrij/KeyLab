using api.v1.main.DTOs.User;

namespace api.v1.main.Services.User
{
    public interface IUserService
    {
        public void ConfirmEmail(string email);
        public void SignUp(UserSignUpDTO body);
        public JWTTokensDTO SignIn(UserSignInDTO body);
        public string UpdateAccessToken(string refreshToken);
    }
}