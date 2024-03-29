using api.v1.main.DTOs;
using api.v1.main.DTOs.Kit;

using component.v1.exceptions;

using db.v1.main.DTOs.Kit;
using db.v1.main.Repositories.Keycap;
using db.v1.main.Repositories.Kit;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.time;

namespace api.v1.main.Services.Kit
{
    public sealed class KitService(IKitRepository kit, IFileConfigurationHelper fileCfg, ILocalizationHelper localization, IUserRepository user, 
        ITimeHelper time, IKitRegexHelper rgx, IFileHelper file, IKeycapRepository keycap, IActivityConfigurationHelper activityCfg, 
        ICacheHelper cache, ICacheConfigurationHelper cacheCfg, IMessageBrokerHelper broker) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IKitService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IKeycapRepository _keycap = keycap;

        private readonly ITimeHelper _time = time;
        private readonly IKitRegexHelper _rgx = rgx;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;

        public async Task<Guid> CreateKit(PostKitDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKitTitle(body.Title);

            var currentTime = _time.GetCurrentUNIXTime();
            var kitID =  _kit.InsertKit(userID, body.Title, currentTime);
            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
            return kitID;
        }

        public async Task UpdateKit(PutKitDTO body, Guid userID, Guid statsID)
        {
            ValidateKitOwner(body.KitID, userID);
            ValidateKitTitle(body.Title);

            _kit.UpdateKit(body.KitID, body.Title);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }

        public async Task DeleteKit(DeleteKitDTO body, Guid userID, Guid statsID) 
        {
            ValidateKitOwner(body.KitID, userID);

            var keycaps = _keycap.SelectKeycaps(body.KitID);
            foreach (var keycap in keycaps)
            {
                var fileName = _keycap.SelectKeycapFileName(keycap.ID);
                var filePath = _fileCfg.GetKeycapFilePath(userID, body.KitID, fileName!);
                _file.DeleteFile(filePath);
                var cacheKey = _cacheCfg.GetFileCacheKey(filePath);
                _cache.DeleteValue(cacheKey);

                var previewName = _keycap.SelectKeycapPreviewName(keycap.ID);
                var previewPath = _fileCfg.GetKeycapFilePath(userID, body.KitID, previewName!);
                _file.DeleteFile(previewPath);
                cacheKey = _cacheCfg.GetFileCacheKey(previewPath);
                _cache.DeleteValue(cacheKey);

                _keycap.DeleteKeycap(keycap.ID);
            }

            _kit.DeleteKit(body.KitID);
            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }



        public async Task<List<SelectKitDTO>> GetDefaultKits(PaginationDTO body, Guid statsID)
        {
            var kits = GetPaginationListOfObjects(body.Page, body.PageSize, _fileCfg.GetDefaultModelsUserID(), _kit.SelectUserKits);
            await PublishActivity(statsID, _activityCfg.GetSeeKeycapActivityTag);
            return kits;
        }

        public async Task<List<SelectKitDTO>> GetUserKits(PaginationDTO body, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);

            var kits = GetPaginationListOfObjects(body.Page, body.PageSize, userID, _kit.SelectUserKits);
            await PublishActivity(statsID, _activityCfg.GetSeeKeycapActivityTag);
            return kits;
        }



        public int GetDefaultKitsTotalPages(int pageSize)
        {
            var totalPages = GetPaginationTotalPages(pageSize, _fileCfg.GetDefaultModelsUserID(), _kit.SelectCountOfKits);
            return totalPages;
        }

        public int GetUserKitsTotalPages(int pageSize, Guid userID)
        {
            ValidateUserID(userID);

            var totalPages = GetPaginationTotalPages(pageSize, userID, _kit.SelectCountOfKits);
            return totalPages;
        }



        private void ValidateKitOwner(Guid kitID, Guid userID)
        {
            if (!_kit.IsKitOwner(kitID, userID))
                throw new BadRequestException(_localization.UserIsNotKitOwner());
        }

        private void ValidateKitTitle(string title)
        {
            _rgx.ValidateKitTitle(title);
        }
    }
}