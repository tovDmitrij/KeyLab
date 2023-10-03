namespace api.v1.main.DTOs.User
{
    public sealed record UserSignUpDTO(string email, string password, string nickname, int emailCode);
}