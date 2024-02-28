using component.v1.exceptions;

using db.v1.main.DTOs.Box;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.User;

using service.v1.cache;
using service.v1.configuration.Interfaces;

namespace api.v1.main.Services.Box
{
    public sealed class BoxService : IBoxService
    {
        private readonly IBoxRepository _boxes;
        private readonly IUserRepository _users;

        private readonly ICacheService _cache;
        private readonly IFileConfigurationService _cfg;

        public BoxService(IBoxRepository boxes, IUserRepository users, ICacheService cache,
                          IFileConfigurationService cfg)
        {
            _boxes = boxes;
            _users = users;
            _cache = cache;
            _cfg = cfg;
        }



        public List<BoxInfoDTO> GetDefaultBoxesList() => GetBoxesList(_cfg.GetDefaultModelsUserID())!;

        public List<BoxInfoDTO>? GetUserBoxesList(Guid userID) => GetBoxesList(userID);



        private List<BoxInfoDTO> GetBoxesList(Guid userID)
        {
            ValidateUserID(userID);

            if (!_cache.TryGetValue($"{userID}/boxes", out List<BoxInfoDTO>? boxes))
            {
                boxes = _boxes.GetUserBoxes(userID);
                _cache.SetValue($"{userID}/boxes", boxes);
            }
            return boxes!;
        }

        private void ValidateUserID(Guid userID)
        {
            if (!_users.IsUserExist(userID))
                throw new BadRequestException("Заданного пользователя не существует");
        }
    }
}