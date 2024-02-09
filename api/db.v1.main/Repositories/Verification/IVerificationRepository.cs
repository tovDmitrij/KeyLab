using db.v1.main.DTOs;

namespace db.v1.main.Repositories.Verification
{
    public interface IVerificationRepository
    {
        public void InsertEmailCode(EmailVerificationDTO body);
        public bool IsEmailCodeValid(EmailVerificationDTO body);
    }
}