using api.v1.users.DTOs;
using component.v1.exceptions;

namespace api.v1.users.Services.Verification
{
    public interface IVerificationService
    {
        /// <exception cref="BadRequestException"></exception>
        public Task SendVerificationEmailCode(ConfirmEmailDTO body);
    }
}