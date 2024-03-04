using component.v1.exceptions;

namespace helper.v1.regex.Interfaces
{
    public interface IBoxRegexHelper
    {
        /// <exception cref="BadRequestException"></exception>
        public void ValidateBoxTitle(string title);

        /// <exception cref="BadRequestException"></exception>
        public void ValidateBoxDescription(string description);
    }
}