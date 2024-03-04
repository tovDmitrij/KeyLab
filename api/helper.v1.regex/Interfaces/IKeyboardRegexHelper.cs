using component.v1.exceptions;

namespace helper.v1.regex.Interfaces
{
    public interface IKeyboardRegexHelper
    {
        /// <exception cref="BadRequestException"></exception>
        public void ValidateKeyboardTitle(string title);

        /// <exception cref="BadRequestException"></exception>
        public void ValidateKeyboardDescription(string description);
    }
}