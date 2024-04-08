using api.v1.main.DTOs.User;

using component.v1.exceptions;

namespace api.v1.main.Services.Verification
{
    public interface IVerificationService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task<string> SendVerificationEmailCode(ConfirmEmailDTO body);
    }
}