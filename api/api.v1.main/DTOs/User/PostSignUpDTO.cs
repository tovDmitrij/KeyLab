namespace api.v1.main.DTOs.User
{
    public sealed record PostSignUpDTO(string Email, string Password, string RepeatedPassword, string Nickname, string EmailCode);
}