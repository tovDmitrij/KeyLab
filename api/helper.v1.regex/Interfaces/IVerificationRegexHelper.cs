using component.v1.exceptions;

namespace helper.v1.regex.Interfaces
{
    public interface IVerificationRegexHelper
    {
        /// <exception cref="BadRequestException"></exception>
        public void ValidateUserEmail(string email);
    }
}