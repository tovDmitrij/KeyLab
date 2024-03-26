using api.v1.main.DTOs;
using api.v1.main.Services.BaseAlgorithm;

using component.v1.exceptions;

using db.v1.main.DTOs.Keycap;
using db.v1.main.Repositories.Keycap;
using db.v1.main.Repositories.Kit;

using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Keycap
{
    public class KeycapService(IBaseAlgorithmService @base, IKeycapRepository keycap, IKitRepository kit, 
        ILocalizationHelper localization, IFileConfigurationHelper fileCfg) : IKeycapService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IKeycapRepository _keycap = keycap;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly ILocalizationHelper _localization = localization;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public byte[] GetKeycapFileBytes(Guid keycapID)
        {
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());
            var file = _base.GetFile(keycapID, kitID, _keycap.SelectKeycapFileName, _kit.SelectKitOwnerID, _fileCfg.GetKeycapFilePath);
            return file;
        }

        public string GetKeycapBase64Preview(Guid keycapID)
        {
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());
            var preview = _base.GetFile(keycapID, kitID, _keycap.SelectKeycapPreviewName, _kit.SelectKitOwnerID, _fileCfg.GetKeycapFilePath);
            return Convert.ToBase64String(preview);
        }

        public List<SelectKeycapDTO> GetKeycaps(PaginationDTO body, Guid kitID)
        {
            ValidateKitID(kitID);

            var keycaps = _base.GetPaginationListOfObjects(body.Page, body.PageSize, kitID, _keycap.SelectKeycaps);
            return keycaps;
        }

        public int GetKeycapsTotalPages(int pageSize, Guid kitID)
        {
            ValidateKitID(kitID);

            var totalPages = _base.GetPaginationTotalPages(pageSize, kitID, _keycap.SelectCountOfKeycaps);
            return totalPages;
        }



        private void ValidateKitID(Guid kitID)
        {
            if (!_kit.IsKitExist(kitID))
                throw new BadRequestException(_localization.KitIsNotExist());
        }
    }
}