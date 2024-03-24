using api.v1.main.DTOs;
using api.v1.main.Services.Base;

using db.v1.main.DTOs.Kit;
using db.v1.main.Repositories.Kit;

using helper.v1.configuration.Interfaces;

namespace api.v1.main.Services.Kit
{
    public sealed class KitService : IKitService
    {
        private readonly IKitRepository _kit;
        private readonly IFileConfigurationHelper _fileCfg;
        private readonly IBaseService _base;

        public KitService(IKitRepository kit, IFileConfigurationHelper fileCfg, IBaseService @base)
        {
            _kit = kit;
            _fileCfg = fileCfg;
            _base = @base;
        }



        public List<SelectKitDTO> GetDefaultKits(PaginationDTO body)
        {
            var kits = _base.GetPaginationListOfObjects(body.Page, body.PageSize, _fileCfg.GetDefaultModelsUserID(), _kit.SelectUserKits);
            return kits;
        }

        public List<SelectKitDTO> GetUserKits(PaginationDTO body, Guid userID)
        {
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
            var totalPages = _base.GetPaginationTotalPages(pageSize, userID, _kit.SelectCountOfKits);
            return totalPages;
        }
    }
}