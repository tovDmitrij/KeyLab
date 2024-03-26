using api.v1.main.DTOs;
using api.v1.main.Services.BaseAlgorithm;

using component.v1.exceptions;

using db.v1.main.DTOs.Kit;
using db.v1.main.Repositories.Kit;
using db.v1.main.Repositories.User;

using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Kit
{
    public sealed class KitService(IKitRepository kit, IFileConfigurationHelper fileCfg, IBaseAlgorithmService @base, 
        ILocalizationHelper localization, IUserRepository user) : IKitService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IUserRepository _user = user;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;
        private readonly ILocalizationHelper _localization = localization;

        public List<SelectKitDTO> GetDefaultKits(PaginationDTO body)
        {
            var kits = _base.GetPaginationListOfObjects(body.Page, body.PageSize, _fileCfg.GetDefaultModelsUserID(), _kit.SelectUserKits);
            return kits;
        }

        public List<SelectKitDTO> GetUserKits(PaginationDTO body, Guid userID)
        {
            ValidateUserID(userID);

            var kits = _base.GetPaginationListOfObjects(body.Page, body.PageSize, userID, _kit.SelectUserKits);
            return kits;
        }



        public int GetDefaultKitsTotalPages(int pageSize)
        {
            var totalPages = _base.GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _kit.SelectCountOfKits);
            return totalPages;
        }

        public int GetUserKitsTotalPages(int pageSize, Guid userID)
        {
            ValidateUserID(userID);

            var totalPages = _base.GetPaginationTotalPages(pageSize, userID, _kit.SelectCountOfKits);
            return totalPages;
        }



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }
    }
}