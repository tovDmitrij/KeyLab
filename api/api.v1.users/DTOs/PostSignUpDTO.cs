namespace api.v1.users.DTOs
{
    public sealed record PostSignUpDTO(string Email, string Password, string RepeatedPassword, string Nickname, string EmailCode);
}