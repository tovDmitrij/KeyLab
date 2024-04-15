namespace api.v1.users.DTOs
{
    public sealed record SignInDTO(string AccessToken, string RefreshToken, bool IsAdmin);
}