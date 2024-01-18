using api.v1.main.DTOs.User;

namespace api.v1.main.Services.Verification
{
    public interface IVerificationService
    {
        public void SendVerificationEmailCode(ConfirmEmailDTO body);
    }
}