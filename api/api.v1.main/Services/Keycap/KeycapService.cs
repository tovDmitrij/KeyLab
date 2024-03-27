using api.v1.main.DTOs;
using api.v1.main.DTOs.Keycap;
using api.v1.main.Services.BaseAlgorithm;

using component.v1.exceptions;

using db.v1.main.DTOs.Keycap;
using db.v1.main.Repositories.Keycap;
using db.v1.main.Repositories.Kit;
using db.v1.main.Repositories.User;

using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;
using helper.v1.time;

namespace api.v1.main.Services.Keycap
{
    public class KeycapService(IBaseAlgorithmService @base, IKeycapRepository keycap, IKitRepository kit, 
        ILocalizationHelper localization, IFileConfigurationHelper fileCfg, IUserRepository user, ITimeHelper time) : IKeycapService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IKeycapRepository _keycap = keycap;
        private readonly IUserRepository _user = user;

        private readonly IBaseAlgorithmService _base = @base;
        private readonly ILocalizationHelper _localization = localization;
        private readonly ITimeHelper _time = time;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public void AddKeycap(PostKeycapDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKitID(body.KitID);

            var names = _base.AddFile(body.File, body.Preview, body.UserID, body.KitID, body.Title, _fileCfg.GetKeycapFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeycapBody = new InsertKeycapDTO(body.KitID, body.Title, names.FileName, names.PreviewName, currentTime);
            _keycap.InsertKeycap(insertKeycapBody);
        }

        public void UpdateKeycap(PutKeycapDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKeycapID(body.KeycapID);
            var kitID = _keycap.SelectKitIDByKeycapID(body.KeycapID);

            var names = _base.AddFile(body.File, body.Preview, body.UserID, (Guid)kitID!, body.Title, _fileCfg.GetKeycapFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var updateKeycapBody = new UpdateKeycapDTO(body.KeycapID, body.Title, names.FileName, names.PreviewName);
            _keycap.UpdateKeycap(updateKeycapBody);
        }



        public byte[] GetKeycapFileBytes(Guid keycapID)
        {
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());

            var fileName = _keycap.SelectKeycapFileName(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _kit.SelectKitOwnerID(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = _fileCfg.GetKeycapFilePath(userID, kitID, fileName);

            var file = _base.GetFile(filePath);
            return file;
        }

        public string GetKeycapBase64Preview(Guid keycapID)
        {
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());

            var fileName = _keycap.SelectKeycapPreviewName(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _kit.SelectKitOwnerID(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = _fileCfg.GetKeycapFilePath(userID, kitID, fileName);

            var preview = _base.GetFile(filePath);
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



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }

        private void ValidateKitID(Guid kitID)
        {
            if (!_kit.IsKitExist(kitID))
                throw new BadRequestException(_localization.KitIsNotExist());
        }

        private void ValidateKeycapID(Guid keycapID)
        {
            if (!_keycap.IsKeycapExist(keycapID))
                throw new BadRequestException(_localization.KeycapIsNotExist());
        }
    }
}