using Microsoft.Extensions.Configuration;
using helper.v1.configuration.Interfaces;

namespace helper.v1.configuration
{
    public sealed class ConfigurationHelper : IEmailConfigurationHelper, IJWTConfigurationHelper, IMinioConfigurationHelper, 
                                              IFileConfigurationHelper, ICacheConfigurationHelper
    {
        private readonly IConfiguration _cfg;

        public ConfigurationHelper(IConfiguration cfg) => _cfg = cfg;



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



        public string GetMinioEndpoint() => _cfg["Minio:Endpoint"] ?? 
            throw new ArgumentNullException("Minio:Endpoint отсутствует в конфигурационном файле");
        public int GetMinioPort() => Convert.ToInt32(_cfg["Minio:Port"] ?? 
            throw new ArgumentNullException("Minio:Port отсутствует в конфигурационном файле"));
        public string GetMinioAccessKey() => _cfg["Minio:AccessKey"] ?? 
            throw new ArgumentNullException("Minio:AccessKey отсутствует в конфигурационном файле");
        public string GetMinioSecretKey() => _cfg["Minio:SecretKey"] ?? 
            throw new ArgumentNullException("Minio:SecretKey отсутствует в конфигурационном файле");



        public string GetEmailHost() => _cfg["Email:Host"] ?? 
            throw new ArgumentNullException("Email:Host отсутствует в конфигурационном файле");
        public int GetEmailPort() => Convert.ToInt32(_cfg["Email:Port"] ??
            throw new ArgumentNullException("Email:Port отсутствует в конфигурационном файле"));
        public string GetEmailLogin() => _cfg["Email:Login"] ?? 
            throw new ArgumentNullException("Email:Login отсутствует в конфигурационном файле");
        public string GetEmailPassword() => _cfg["Email:Password"] ?? 
            throw new ArgumentNullException("Email:Password отсутствует в конфигурационном файле");



        public string GetModelsParentDirectory() => _cfg["File:ModelsParentDirectory"] ??
            throw new ArgumentNullException("File:ModelsParentDirectory отсутствует в конфигурационном файле");
        public Guid GetDefaultModelsUserID() => Guid.Parse(_cfg["File:DefaultModelsUserID"] ??
            throw new ArgumentNullException("File:DefaultModelsUserID отсутствует в конфигурационном файле"));
        public string GetSwitchModelsDirectory() => _cfg["File:SwitchModelDirectory"] ??
            throw new ArgumentNullException("File:SwitchModelDirectory отсутствует в конфигурационном файле");
        public string GetSwitchSoundsDirectory() => _cfg["File:SwitchSoundDirectory"] ?? 
            throw new ArgumentNullException("File:SwitchSoundDirectory отсутствует в конфигурационном файле");



        public int GetCacheExpirationMinutes() => Convert.ToInt32(_cfg["Cache:ExpirationMinutes"] ?? 
            throw new ArgumentNullException("Cache:ExpirationMinutes отсутствует в конфигурационном файле"));
    }
}