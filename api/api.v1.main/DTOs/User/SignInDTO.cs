namespace api.v1.main.DTOs.User
{
    public sealed record SignInDTO(string AccessToken, string RefreshToken, bool IsAdmin);
}