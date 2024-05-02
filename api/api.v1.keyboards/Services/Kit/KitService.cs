using api.v1.keyboards.DTOs.Kit;

using component.v1.exceptions;

using db.v1.keyboards.DTOs;
using db.v1.keyboards.Repositories.Box;
using db.v1.keyboards.Repositories.Keycap;
using db.v1.keyboards.Repositories.Kit;
using db.v1.users.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.time;

namespace api.v1.keyboards.Services.Kit
{
    public sealed class KitService(IKitRepository kit, IFileConfigurationHelper fileCfg, ILocalizationHelper localization, IUserRepository user, 
        ITimeHelper time, IKitRegexHelper rgx, IFileHelper file, IKeycapRepository keycap, IActivityConfigurationHelper activityCfg, 
        ICacheHelper cache, ICacheConfigurationHelper cacheCfg, IMessageBrokerHelper broker, IBoxRepository box) : 
        BaseAlgorithmService(localization, user, cache, cacheCfg, file, broker), IKitService
    {
        private readonly IKitRepository _kit = kit;
        private readonly IKeycapRepository _keycap = keycap;
        private readonly IBoxRepository _box = box;
        private readonly ITimeHelper _time = time;
        private readonly IKitRegexHelper _rgx = rgx;
        private readonly IFileConfigurationHelper _fileCfg = fileCfg;
        private readonly IActivityConfigurationHelper _activityCfg = activityCfg;



        public async Task<Guid> CreateKit(Guid boxTypeID, string? title, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKitTitle(title);
            ValidateBoxTypeID(boxTypeID);

            var currentTime = _time.GetCurrentUNIXTime();
            var previewName = Guid.NewGuid().ToString();
            var kitID =  _kit.InsertKit(userID, boxTypeID, title, previewName, currentTime);

            var defaultUserID = _fileCfg.GetDefaultModelsUserID();
            var defaultKitID = _kit.SelectUserKits(defaultUserID, boxTypeID).First().ID;
            var keycaps = _keycap.SelectKeycaps(defaultKitID);

            var fileExtension = _fileCfg.GetModelFilenameExtension();
            var time = _time.GetCurrentUNIXTime();
            foreach (var keycap in keycaps)
            {
                var fileName = _keycap.SelectKeycapFileName(keycap.ID);
                var defaultFilePath = _fileCfg.GetKeycapFilePath(defaultUserID, defaultKitID, fileName!, fileExtension);

                var newFilePath = _fileCfg.GetKeycapFilePath(userID, kitID, fileName!, fileExtension);

                _file.CopyFile(defaultFilePath, newFilePath);

                _keycap.InsertKeycap(kitID, keycap.Title, fileName!, time);
            }

            var defaultPreviewName = _kit.SelectKitPreviewName(defaultKitID);
            var previewExtension = _fileCfg.GetPreviewFilenameExtension();
            var defaultPreviewFilePath = _fileCfg.GetKeycapFilePath(defaultUserID, defaultKitID, defaultPreviewName!, previewExtension);
            var newPreviewFilePath = _fileCfg.GetKeycapFilePath(userID, kitID, previewName, previewExtension);

            _file.CopyFile(defaultPreviewFilePath, newPreviewFilePath);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
            return kitID;
        }

        public async Task PatchKitTitle(Guid kitID, string title, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKitOwner(kitID, userID);
            ValidateKitTitle(title);

            var currentTime = _time.GetCurrentUNIXTime();
            _kit.UpdateKit(kitID, title, currentTime);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }

        public async Task PatchKitPreview(IFormFile? preview, Guid kitID, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateKitOwner(kitID, userID);

            UpdateFile(preview, userID, kitID, kitID, _kit.SelectKitPreviewName!, _fileCfg.GetPreviewFilenameExtension,
                _fileCfg.GetKeycapFilePath);

            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }

        public async Task DeleteKit(DeleteKitDTO body, Guid userID, Guid statsID) 
        {
            ValidateKitOwner(body.KitID, userID);

            var keycaps = _keycap.SelectKeycaps(body.KitID);
            var fileExtension = _fileCfg.GetModelFilenameExtension();
            var previewExtension = _fileCfg.GetPreviewFilenameExtension();
            foreach (var keycap in keycaps)
            {
                var fileName = _keycap.SelectKeycapFileName(keycap.ID);
                var filePath = _fileCfg.GetKeycapFilePath(userID, body.KitID, fileName!, fileExtension);
                _file.DeleteFile(filePath);

                var previewPath = _fileCfg.GetKeycapFilePath(userID, body.KitID, fileName!, previewExtension);
                _file.DeleteFile(previewPath);

                var cacheKey = _cacheCfg.GetFileCacheKey(filePath);
                _cache.DeleteValue(cacheKey);

                cacheKey = _cacheCfg.GetFileCacheKey(previewPath);
                _cache.DeleteValue(cacheKey);

                _keycap.DeleteKeycap(keycap.ID);
            }

            var kitPreviewName = _kit.SelectKitPreviewName(body.KitID);
            var kitPreviewPath = _fileCfg.GetKeycapFilePath(userID, body.KitID, kitPreviewName!, previewExtension);
            _file.DeleteFile(kitPreviewPath);

            _kit.DeleteKit(body.KitID);
            await PublishActivity(statsID, _activityCfg.GetEditKeycapActivityTag);
        }



        public async Task<string> GetKitPreviewBase64(Guid kitID)
        {
            ValidateKitID(kitID);

            var userID = _kit.SelectKitOwnerID(kitID);
            var previewName = _kit.SelectKitPreviewName(kitID);
            var previewExtension = _fileCfg.GetPreviewFilenameExtension();
            var previewPath = _fileCfg.GetKeycapFilePath((Guid)userID!, kitID, previewName!, previewExtension);

            var preview = await ReadFile(previewPath);
            return Convert.ToBase64String(preview);
        }



        public async Task<List<SelectKitDTO>> GetDefaultKits(int page, int pageSize, Guid boxTypeID, Guid statsID)
        {
            ValidateBoxTypeID(boxTypeID);

            var kits = GetPaginationListOfObjects(page, pageSize, _fileCfg.GetDefaultModelsUserID(), boxTypeID, _kit.SelectUserKits);
            await PublishActivity(statsID, _activityCfg.GetSeeKeycapActivityTag);
            return kits;
        }

        public async Task<List<SelectKitDTO>> GetUserKits(int page, int pageSize, Guid boxTypeID, Guid userID, Guid statsID)
        {
            ValidateUserID(userID);
            ValidateBoxTypeID(boxTypeID);

            var kits = GetPaginationListOfObjects(page, pageSize, userID, boxTypeID, _kit.SelectUserKits);
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

        private void ValidateKitID(Guid kitID)
        {
            if (!_kit.IsKitExist(kitID))
                throw new BadRequestException(_localization.KitIsNotExist());
        }

        private void ValidateKitTitle(string title)
        {
            _rgx.ValidateKitTitle(title);
        }
        
        private void ValidateBoxTypeID(Guid boxTypeID)
        {
            if (!_box.IsBoxTypeExist(boxTypeID))
                throw new BadRequestException(_localization.BoxTypeIsNotExist());
        }
    }
}