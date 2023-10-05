namespace api.v1.main.DTOs.User
{
    public sealed record JWTTokensDTO(string AccessToken, string RefreshToken);
}