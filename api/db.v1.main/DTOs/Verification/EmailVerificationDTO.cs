namespace db.v1.main.DTOs.Verification
{
    public sealed record EmailVerificationDTO(string Email, string Code, double Date);
}