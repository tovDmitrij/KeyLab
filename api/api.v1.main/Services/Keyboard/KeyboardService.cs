﻿using api.v1.main.DTOs;
using api.v1.main.DTOs.Keyboard;

using component.v1.exceptions;
using component.v1.preview;

using db.v1.main.DTOs.Keyboard;
using db.v1.main.Repositories.Box;
using db.v1.main.Repositories.Keyboard;
using db.v1.main.Repositories.Switch;
using db.v1.main.Repositories.User;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;
using helper.v1.messageBroker;
using helper.v1.regex.Interfaces;
using helper.v1.time;

namespace api.v1.main.Services.Keyboard
{
    public sealed class KeyboardService : IKeyboardService
    {
        private readonly IKeyboardRepository _keyboard;
        private readonly IUserRepository _user;
        private readonly IBoxRepository _box;
        private readonly ISwitchRepository _switch;

        private readonly IFileConfigurationHelper _fileCfg;
        private readonly IPreviewConfigurationHelper _previewCfg;
        private readonly IKeyboardRegexHelper _rgx;
        private readonly ICacheHelper _cache;
        private readonly IFileHelper _file;
        private readonly ITimeHelper _time;
        private readonly ILocalizationHelper _localization;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly IMessageBrokerHelper _broker;

        public KeyboardService(IFileHelper file, ITimeHelper time, IKeyboardRepository keyboard, 
                               IUserRepository user, IKeyboardRegexHelper rgx, ICacheHelper cache,
                               IFileConfigurationHelper fileCfg, IBoxRepository box, ISwitchRepository @switch,
                               ILocalizationHelper localization, ICacheConfigurationHelper cacheCfg,
                               IMessageBrokerHelper broker, IPreviewConfigurationHelper previewCfg)
        {
            _file = file;
            _time = time;
            _keyboard = keyboard;
            _user = user;
            _rgx = rgx;
            _cache = cache;
            _fileCfg = fileCfg;
            _box = box;
            _switch = @switch;
            _localization = localization;
            _cacheCfg = cacheCfg;
            _broker = broker;
            _previewCfg = previewCfg;
        }



        public async Task AddKeyboard(PostKeyboardDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKeyboardFile(body.File);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateKeyboardDescription(body.Description);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);

            var modelFileName = $"{body.Title}.glb";
            var modelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, modelFileName);

            using var memoryStream = new MemoryStream();
            body.File!.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();

            var fileType = _previewCfg.GetPreviewFileType();

            var imgFileName = $"{body.Title}.{fileType}";
            var imgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, imgFileName);
            var previewBody = new PreviewDTO(imgFilePath, bytes);
            await _broker.SendData(previewBody);

            var currentTime = _time.GetCurrentUNIXTime();
            var insertKeyboardBody = new InsertKeyboardDTO(body.UserID, body.SwitchTypeID, body.BoxTypeID, body.Title, 
                body.Description, modelFileName, imgFileName, currentTime);
            _keyboard.InsertKeyboardInfo(insertKeyboardBody);

            _file.AddFile(bytes, modelFilePath);
        }

        public async Task UpdateKeyboard(PutKeyboardDTO body)
        {
            ValidateKeyboardExist(body.KeyboardID);
            ValidateKeyboardFile(body.File);
            ValidateKeyboardTitle(body.UserID, body.Title);
            ValidateKeyboardDescription(body.Description);
            ValidateBoxType(body.BoxTypeID);
            ValidateSwitchType(body.SwitchTypeID);
            ValidateUserID(body.UserID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);


            var oldModelFileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var oldModelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, oldModelFileName);

            var newModelFileName = $"{body.Title}.glb";
            var newModelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, newModelFileName);


            using var memoryStream = new MemoryStream();
            body.File!.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();


            var oldImgFileName = _keyboard.SelectKeyboardPreviewName(body.KeyboardID)!;
            var oldImgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, oldImgFileName);

            var fileType = _previewCfg.GetPreviewFileType();

            var newImgFileName = $"{body.Title}.{fileType}";
            var newImgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, newImgFileName);


            var previewBody = new PreviewDTO(oldImgFilePath, bytes);
            _file.DeleteFile(oldImgFilePath);
            await _broker.SendData(previewBody);


            var updateKeyboardBody = new UpdateKeyboardDTO(body.KeyboardID, body.SwitchTypeID, body.BoxTypeID, body.Title, body.Description, newModelFileName, newImgFileName);
            _keyboard.UpdateKeyboardInfo(updateKeyboardBody);


            _file.UpdateFile(bytes, oldModelFilePath);
            _file.MoveFile(oldModelFilePath, newModelFilePath);
        }
        
        public void DeleteKeyboard(DeleteKeyboardDTO body)
        {
            ValidateUserID(body.UserID);
            ValidateKeyboardExist(body.KeyboardID);
            ValidateKeyboardOwner(body.KeyboardID, body.UserID);

            var modelFileName = _keyboard.SelectKeyboardFileName(body.KeyboardID)!;
            var modelFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, modelFileName);

            var imgFileName = _keyboard.SelectKeyboardPreviewName(body.KeyboardID)!;
            var imgFilePath = _fileCfg.GetKeyboardModelFilePath(body.UserID, imgFileName);

            _file.DeleteFile(modelFilePath);
            _file.DeleteFile(imgFilePath);
            _keyboard.DeleteKeyboardInfo(body.KeyboardID);
            _cache.DeleteValue(body.KeyboardID);
        }

        public byte[] GetKeyboardFile(Guid keyboardID)
        {
            var fileName = _keyboard.SelectKeyboardFileName(keyboardID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var userID = _keyboard.SelectKeyboardOwnerID(keyboardID) ?? throw new BadRequestException(_localization.FileIsNotExist());

            var filePath = _fileCfg.GetKeyboardModelFilePath(userID, fileName);

            if (!_cache.TryGetValue(keyboardID, out byte[]? file))
            {
                file = _file.GetFile(filePath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(keyboardID, file, minutes);
            }
            return file!;
        }



        public List<KeyboardListDTO> GetDefaultKeyboardsList(PaginationDTO body) => GetKeyboardsList(body, _fileCfg.GetDefaultModelsUserID())!;
        public List<KeyboardListDTO> GetUserKeyboardsList(PaginationDTO body, Guid userID) => GetKeyboardsList(body, userID);

        public int GetDefaultKeyboardsTotalPages(int pageSize)
        {
            var userID = _fileCfg.GetDefaultModelsUserID();
            var count = _keyboard.SelectCountOfKeyboards(userID);
            double totalPages = count / pageSize;
            return (int)Math.Ceiling(totalPages);
        }

        public int GetUserKeyboardsTotalPages(Guid userID, int pageSize)
        {
            ValidateUserID(userID);
            var count = _keyboard.SelectCountOfKeyboards(userID);
            double totalPages = count / pageSize;
            return (int)Math.Ceiling(totalPages);
        }

        private List<KeyboardListDTO> GetKeyboardsList(PaginationDTO body, Guid userID)
        {
            ValidateUserID(userID);

            var keyboards = new List<KeyboardListDTO>();

            var fileType = _previewCfg.GetPreviewFileType();
            var dbKeyboards = _keyboard.SelectUserKeyboards(body.Page, body.PageSize, userID);
            foreach (var keyboard in dbKeyboards)
            {
                var filePath = _fileCfg.GetKeyboardModelFilePath(userID, keyboard.PreviewName);

                byte[] bytes;
                try
                {
                    bytes = _file.GetFile(filePath);
                }
                catch
                {
                    var errorImgPath = _fileCfg.GetErrorImageFilePath();
                    bytes = _file.GetFile(errorImgPath);
                }
                var img = $"data:image/{fileType};base64," + Convert.ToBase64String(bytes);

                keyboards.Add(new(keyboard.ID, keyboard.BoxTypeID, keyboard.BoxTypeTitle, keyboard.SwitchTypeID,
                    keyboard.SwitchTypeTitle, keyboard.Title, keyboard.Description, img, keyboard.CreationDate));
            }

            return keyboards;
        }



        private void ValidateUserID(Guid userID)
        {
            if (!_user.IsUserExist(userID))
                throw new BadRequestException(_localization.UserIsNotExist());
        }

        private void ValidateKeyboardFile(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException(_localization.FileIsNotAttached());
        }

        private void ValidateKeyboardDescription(string? description)
        {
            if (description != null)
                _rgx.ValidateKeyboardDescription(description);
        }

        private void ValidateKeyboardTitle(Guid userID, string title)
        {
            if (_keyboard.IsKeyboardTitleBusy(userID, title))
                throw new BadRequestException(_localization.KeyboardTitleIsBusy());
            _rgx.ValidateKeyboardTitle(title);
        }

        private void ValidateBoxType(Guid boxTypeID)
        {
            if (!_box.IsBoxTypeExist(boxTypeID))
                throw new BadRequestException(_localization.BoxTypeIsNotExist());
        }

        private void ValidateSwitchType(Guid switchTypeID)
        {
            if (!_switch.IsSwitchExist(switchTypeID))
                throw new BadRequestException(_localization.SwitchTypeIsNotExist());
        }

        private void ValidateKeyboardExist(Guid keyboardID)
        {
            if (!_keyboard.IsKeyboardExist(keyboardID))
                throw new BadRequestException(_localization.FileIsNotExist());
        }

        private void ValidateKeyboardOwner(Guid keyboardID, Guid userID)
        {
            if (!_keyboard.IsKeyboardOwner(keyboardID, userID))
                throw new BadRequestException(_localization.UserIsNotKeyboardOwner());
        }   
    }
}