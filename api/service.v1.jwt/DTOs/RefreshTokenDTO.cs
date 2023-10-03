namespace service.v1.jwt.DTOs
{
    public sealed record RefreshTokenDTO(string Value, double CreationDate, double ExpireDate);
}