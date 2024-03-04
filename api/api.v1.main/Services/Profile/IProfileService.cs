using component.v1.exceptions;

namespace api.v1.main.Services.Profile
{
    public interface IProfileService
    {
        /// <exception cref="BadRequestException"></exception>
        public string GetUserNickname(Guid userID);
    }
}