using api.v1.main.DTOs;
using api.v1.main.DTOs.Switch;
using api.v1.main.Services.Base;
using component.v1.exceptions;

using db.v1.main.Repositories.Switch;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.file;
using helper.v1.localization.Helper;

namespace api.v1.main.Services.Switch
{
    public sealed class SwitchService : ISwitchService
    {
        private readonly ISwitchRepository _switch;

        private readonly IFileConfigurationHelper _fileCfg;
        private readonly IPreviewConfigurationHelper _previewCfg;
        private readonly IFileHelper _file;
        private readonly ICacheHelper _cache;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly ILocalizationHelper _localization;
        private readonly IBaseService _base;

        public SwitchService(ISwitchRepository switches, IFileHelper file, IFileConfigurationHelper fileCfg,
                             ICacheHelper cache, ICacheConfigurationHelper cacheCfg, ILocalizationHelper localization,
                             IPreviewConfigurationHelper previewCfg, IBaseService @base)
        {
            _switch = switches;
            _file = file;
            _fileCfg = fileCfg;
            _cache = cache;
            _cacheCfg = cacheCfg;
            _localization = localization;
            _previewCfg = previewCfg;
            _base = @base;
        }



        public byte[] GetSwitchModelFile(Guid switchID)
        {
            var fileName = _switch.SelectSwitchModelName(switchID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = _fileCfg.GetSwitchModelFilePath(fileName);

            if (!_cache.TryGetValue(filePath, out byte[]? file))
            {
                file = _file.GetFile(filePath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(filePath, file, minutes);
            }
            return file!;
        }

        public string GetSwitchSoundBase64(Guid switchID)
        {
            var fileName = _switch.SelectSwitchSoundName(switchID) ?? throw new BadRequestException(_localization.FileIsNotExist());
            var filePath = _fileCfg.GetSwitchModelFilePath(fileName);

            if (!_cache.TryGetValue(filePath, out byte[]? file))
            {
                file = _file.GetFile(filePath);
                if (file.Length == 0)
                    throw new BadRequestException(_localization.FileIsNotExist());

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(filePath, file, minutes);
            }

            var fileType = _previewCfg.GetPreviewFileType();
            var base64File = $"data:audio/{fileType};base64," + Convert.ToBase64String(file!);
            return base64File;
        }



        public List<SwitchListDTO> GetSwitches(PaginationDTO body)
        {
            ValidatePageSize(body.PageSize);
            ValidatePage(body.Page);

            var switches = new List<SwitchListDTO>();

            var fileType = _previewCfg.GetPreviewFileType();
            var dbSwitches = _switch.SelectSwitches(body.Page, body.PageSize);
            foreach (var sw in dbSwitches)
            {
                var filePath = _fileCfg.GetSwitchModelFilePath(sw.PreviewName);

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

                switches.Add(new(sw.ID, sw.Title, img));
            }

            return switches!;
        }
        public int GetSwitchesTotalPages(int pageSize) => 
            _base.GetPaginationTotalPages(pageSize, _switch.SelectCountOfSwitch);

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