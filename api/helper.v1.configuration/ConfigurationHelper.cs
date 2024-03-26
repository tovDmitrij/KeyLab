using Microsoft.Extensions.Configuration;
using helper.v1.configuration.Interfaces;

namespace helper.v1.configuration
{
    public sealed class ConfigurationHelper(IConfiguration cfg) : 
        IEmailConfigurationHelper, IJWTConfigurationHelper, IFileConfigurationHelper, ICacheConfigurationHelper, 
        IPreviewConfigurationHelper, IAdminConfigurationHelper
    {
        private readonly IConfiguration _cfg = cfg;

        public string GetJWTSecretKey() => _cfg["JWT:SecretKey"] ?? 
            throw new ArgumentNullException("JWT:SecretKey отсутствует в конфигурационном файле");
        public double GetJWTAccessExpireDate() => Convert.ToDouble(_cfg["JWT:AccessExpireDate"] ?? 
            throw new ArgumentNullException("JWT:AccessExpireDate отсутствует в конфигурационном файле"));
        public double GetJWTRefreshExpireDate() => Convert.ToDouble(_cfg["JWT:RefreshExpireDate"] ?? 
            throw new ArgumentNullException("JWT:RefreshExpireDate отсутствует в конфигурационном файле"));
        public string GetJWTIssuer() => _cfg["JWT:Issuer"] ?? 
            throw new ArgumentNullException("JWT:Issuer отсутствует в конфигурационном файле");
        public string GetJWTAudience() => _cfg["JWT:Audience"] ?? 
            throw new ArgumentNullException("JWT:Audience отсутствует в конфигурационном файле");

        public string GetEmailHost() => _cfg["Email:Host"] ?? 
            throw new ArgumentNullException("Email:Host отсутствует в конфигурационном файле");
        public int GetEmailPort() => Convert.ToInt32(_cfg["Email:Port"] ??
            throw new ArgumentNullException("Email:Port отсутствует в конфигурационном файле"));
        public string GetEmailLogin() => _cfg["Email:Login"] ?? 
            throw new ArgumentNullException("Email:Login отсутствует в конфигурационном файле");
        public string GetEmailPassword() => _cfg["Email:Password"] ?? 
            throw new ArgumentNullException("Email:Password отсутствует в конфигурационном файле");

        public Guid GetDefaultModelsUserID() => Guid.Parse(_cfg["File:DefaultModelsUserID"] ??
            throw new ArgumentNullException("File:DefaultModelsUserID отсутствует в конфигурационном файле"));
        public string GetSwitchFilePath(string fileName) => string.Format(_cfg["File:SwitchFilePath"] ??
            throw new ArgumentNullException("File:SwitchFilePath отсутствует в конфигурационном файле"), fileName);
        public string GetSwitchSoundFilePath(string fileName) => string.Format(_cfg["File:SwitchSoundPath"] ??
            throw new ArgumentNullException("File:SwitchSoundPath отсутствует в конфигурационном файле"), fileName);
        public string GetKeyboardFilePath(Guid userID, string fileName) => string.Format(_cfg["File:KeyboardFilePath"] ??
            throw new ArgumentNullException("File:KeyboardFilePath отсутствует в конфигурационном файле"), userID, fileName);
        public string GetBoxFilePath(Guid userID, string fileName) => string.Format(_cfg["File:BoxFilePath"] ??
            throw new ArgumentNullException("File:BoxFilePath отсутствует в конфигурационном файле"), userID, fileName);

        public string GetKeycapFilePath(Guid userID, Guid kitID, string fileName) => string.Format(_cfg["File:KeycapFilePath"] ??
            throw new ArgumentNullException("File:KeycapFilePath отсутствует в конфигурационном файле"), userID, kitID, fileName);

        public int GetCacheExpirationMinutes() => Convert.ToInt32(_cfg["Cache:ExpirationMinutes"] ?? 
            throw new ArgumentNullException("Cache:ExpirationMinutes отсутствует в конфигурационном файле"));

        public string GetPreviewFileType() => _cfg["FilePreview:ImageType"] ??
            throw new ArgumentNullException("FilePreview:ImageType отсутствует в конфигурационном файле");

        public Guid GetDefaultUserID() => Guid.Parse(_cfg["File:DefaultModelsUserID"] ??
            throw new ArgumentNullException("File:DefaultModelsUserID отсутствует в конфигурационном файле"));
    }
}