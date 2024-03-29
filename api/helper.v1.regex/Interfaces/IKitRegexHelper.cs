using component.v1.exceptions;

namespace helper.v1.regex.Interfaces
{
    public interface IKitRegexHelper
    {
        /// <exception cref="BadRequestException"></exception>
        public void ValidateKitTitle(string title);
    }
}