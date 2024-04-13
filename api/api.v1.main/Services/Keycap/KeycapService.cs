using api.v1.main.DTOs;
using api.v1.main.DTOs.Keycap;

using component.v1.exceptions;

using db.v1.main.DTOs.Keycap;
using db.v1.main.Repositories.Keycap;
using db.v1.main.Repositories.Kit;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.time;

namespace api.v1.main.Services.Keycap
{
    public class KeycapService(IKeycapRepository keycap, IKitRepository kit, ILocalizationHelper localization, IFileConfigurationHelper fileCfg, 
        IUserRepository user, ITimeHelper time, IActivityConfigurationHelper activityCfg, ICacheHelper cache, ICacheConfigurationHelper cacheCfg,
        IFileHelper file, IMessageBrokerHelper broker) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IKeycapService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IKeycapRepository _keycap = keycap;

        private readonly ITimeHelper _time = time;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;

        public async Task AddKeycap(PostKeycapDTO body, Guid statsID)
        {
            ValidateUserID(body.UserID);
            ValidateKitID(body.KitID);

            var names = UploadFile(body.File, body.Preview, body.UserID, body.KitID, body.Title, _fileCfg.GetKeycapFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeycapBody = new InsertKeycapDTO(body.KitID, body.Title, names.FileName, names.PreviewName, currentTime);
            _keycap.InsertKeycap(insertKeycapBody);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }

        public async Task UpdateKeycap(PutKeycapDTO body, Guid statsID)
        {
            ValidateUserID(body.UserID);
            ValidateKeycapID(body.KeycapID);
            var kitID = _keycap.SelectKitIDByKeycapID(body.KeycapID);

            var names = UploadFile(body.File, body.Preview, body.UserID, (Guid)kitID!, body.Title, _fileCfg.GetKeycapFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            var updateKeycapBody = new UpdateKeycapDTO(body.KeycapID, body.Title, names.FileName, names.PreviewName);
            _keycap.UpdateKeycap(updateKeycapBody);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }

        public async Task PatchKeycapTitle(PatchKeycapTitleDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKeycapID(userID);

            _keycap.UpdateKeycapTitle(body.Title, body.KeycapID);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }



        public async Task<byte[]> GetKeycapFileBytes(Guid keycapID, Guid statsID)
        {
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());

            var fileName = _keycap.SelectKeycapFileName(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _kit.SelectKitOwnerID(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = _fileCfg.GetKeycapFilePath(userID, kitID, fileName);

            var file = await ReadFile(filePath);

            await PublishActivity(statsID, _activityCfg.GetSeeKeycapActivityTag);
            return file;
        }

        public async Task<string> GetKeycapBase64Preview(Guid keycapID)
        {
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());

            var fileName = _keycap.SelectKeycapPreviewName(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _kit.SelectKitOwnerID(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = _fileCfg.GetKeycapFilePath(userID, kitID, fileName);

            var preview = await ReadFile(filePath);
            return Convert.ToBase64String(preview);
        }

        public async Task<List<SelectKeycapDTO>> GetKeycaps(PaginationDTO body, Guid kitID, Guid statsID)
        {
            ValidateKitID(kitID);

            var keycaps = GetPaginationListOfObjects(body.Page, body.PageSize, kitID, _keycap.SelectKeycaps);

            await PublishActivity(statsID, _activityCfg.GetSeeKeycapActivityTag);
            return keycaps;
        }

        public int GetKeycapsTotalPages(int pageSize, Guid kitID)
        {
            ValidateKitID(kitID);

            var totalPages = GetPaginationTotalPages(pageSize, kitID, _keycap.SelectCountOfKeycaps);
            return totalPages;
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