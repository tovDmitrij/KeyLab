using db.v1.users.DTOs.Verification;

namespace db.v1.users.Repositories.Verification
{
    public interface IVerificationRepository
    {
        public void InsertEmailCode(EmailVerificationDTO body);
        public bool IsEmailCodeValid(EmailVerificationDTO body);
    }
}