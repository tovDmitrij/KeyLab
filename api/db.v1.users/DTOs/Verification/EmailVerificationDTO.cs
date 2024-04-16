namespace db.v1.users.DTOs.Verification
{
    public sealed record EmailVerificationDTO(string Email, string Code, double Date);
}