namespace db.v1.main.DTOs.User
{
    public sealed record RefreshTokenDTO(Guid UserID, string RefreshToken, double Date);
}