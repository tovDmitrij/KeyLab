namespace db.v1.main.DTOs
{
    public sealed record EmailVerificationDTO(string Email, string Code, double Date);
}