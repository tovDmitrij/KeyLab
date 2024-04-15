namespace db.v1.users.DTOs.User
{
    public sealed record RefreshTokenDTO(Guid UserID, string RefreshToken, double Date);
}