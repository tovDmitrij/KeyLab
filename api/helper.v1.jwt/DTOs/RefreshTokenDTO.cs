namespace helper.v1.jwt.DTOs
{
    public sealed record RefreshTokenDTO(string Value, double CreationDate, double ExpireDate);
}