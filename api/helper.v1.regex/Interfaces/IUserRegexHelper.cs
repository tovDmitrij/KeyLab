using component.v1.exceptions;

namespace helper.v1.regex.Interfaces
{
    public interface IUserRegexHelper
    {
        /// <exception cref="BadRequestException"></exception>
        public void ValidateUserEmail(string email);
        /// <exception cref="BadRequestException"></exception>
        public void ValidateUserPassword(string password);
        /// <exception cref="BadRequestException"></exception>
        public void ValidateUserNickname(string nickname);
    }
}