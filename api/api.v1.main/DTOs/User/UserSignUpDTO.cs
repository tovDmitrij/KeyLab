namespace api.v1.main.DTOs.User
{
    public sealed record UserSignUpDTO(string Email, string Password, string Nickname, string EmailCode);
}