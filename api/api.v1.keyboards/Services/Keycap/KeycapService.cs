using api.v1.keyboards.DTOs.Keycap;

using component.v1.exceptions;

using db.v1.keyboards.DTOs;
using db.v1.keyboards.Repositories.Keycap;
using db.v1.keyboards.Repositories.Kit;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.localization.Helper.Interfaces;
using helper.v1.messageBroker;
using helper.v1.time;

namespace api.v1.keyboards.Services.Keycap
{
    public class KeycapService(IKeycapRepository keycap, IKitRepository kit, IFileLocalizationHelper localization, IFileConfigurationHelper fileCfg, 
        IUserRepository user, ITimeHelper time, IActivityConfigurationHelper activityCfg, ICacheHelper cache, ICacheConfigurationHelper cacheCfg,
        IFileHelper file, IMessageBrokerHelper broker) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IKeycapService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IKeycapRepository _keycap = keycap;
        private readonly ITimeHelper _time = time;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;



        public async Task UpdateKeycap(IFormFile? file, Guid keycapID, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKeycapID(keycapID);
            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());

            UpdateFile(file, userID, keycapID, kitID, _keycap.SelectKeycapFileName!, _fileCfg.GetModelFilenameExtension, 
                _fileCfg.GetKeycapFilePath);

            var currentTime = _time.GetCurrentUNIXTime();
            _keycap.UpdateKeycap(keycapID, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }



        public async Task<byte[]> GetKeycapFileBytes(Guid keycapID, Guid statsID)
        {
            ValidateKeycapID(keycapID);

            var kitID = _keycap.SelectKitIDByKeycapID(keycapID) ?? throw new BadRequestException(_localization.KitIsNotExist());

            var userID = _kit.SelectKitOwnerID(kitID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var fileName = _keycap.SelectKeycapFileName(keycapID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var fileExtension = _fileCfg.GetModelFilenameExtension();
            var filePath = _fileCfg.GetKeycapFilePath(userID, kitID, fileName, fileExtension);

            var file = await ReadFile(filePath);

            await PublishActivity(statsID, _activityCfg.GetSeeKeycapActivityTag);
            return file;
        }



        public async Task<List<SelectKeycapDTO>> GetKeycaps(int page, int pageSize, Guid kitID, Guid statsID)
        {
            ValidateKitID(kitID);

            var keycaps = GetPaginationListOfObjects(page, pageSize, kitID, _keycap.SelectKeycaps);

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