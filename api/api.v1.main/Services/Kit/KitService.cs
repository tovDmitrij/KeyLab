using api.v1.main.DTOs;
using api.v1.main.Services.Base;

using component.v1.exceptions;

using db.v1.main.DTOs.Kit;
using db.v1.main.Repositories.Kit;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Kit
{
    public sealed class KitService : IKitService
    {
        private readonly IKitRepository _kit;
        private readonly IUserRepository _user;
        private readonly IFileConfigurationHelper _fileCfg;
        private readonly ILocalizationHelper _localization;
        private readonly ICacheHelper _cache;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly IBaseService _base;

        public KitService(IKitRepository kit, IFileConfigurationHelper fileCfg, ILocalizationHelper localization,
                          IUserRepository user, ICacheHelper cache, ICacheConfigurationHelper cacheCfg, IBaseService @base)
        {
            _kit = kit;
            _fileCfg = fileCfg;
            _localization = localization;
            _user = user;
            _cache = cache;
            _cacheCfg = cacheCfg;
            _base = @base;
        }



        public List<SelectKitDTO> GetDefaultKits(PaginationDTO body) => GetKits(body, _fileCfg.GetDefaultModelsUserID());
        public List<SelectKitDTO> GetUserKits(PaginationDTO body, Guid userID) => GetKits(body, userID);

        public int GetDefaultKitsTotalPages(int pageSize) =>
            _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _kit.SelectCountOfKits);
        public int GetUserKitsTotalPages(int pageSize, Guid userID) =>
            _base.GetPaginationTotalPages(pageSize, userID, _kit.SelectCountOfKits);



        private List<SelectKitDTO> GetKits(PaginationDTO body, Guid userID)
        {
            ValidateUserID(userID);
            ValidatePage(body.Page);
            ValidatePageSize(body.PageSize);

            var cacheKey = body.GetHashCode() + userID.GetHashCode();
            if (!_cache.TryGetValue(cacheKey, out List<SelectKitDTO>? kits))
            {
                kits = _kit.SelectUserKits(body.Page, body.PageSize, userID);

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, kits, minutes);
            }

            return kits!;
        }



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }

        private void ValidatePageSize(int pageSize)
        {
            if (pageSize < 1)
                throw new BadRequestException(_localization.PaginationPageSizeIsNotValid());
        }

        private void ValidatePage(int page)
        {
            if (page < 1)
                throw new BadRequestException(_localization.PaginationPageIsNotValid());
        }
    }
}