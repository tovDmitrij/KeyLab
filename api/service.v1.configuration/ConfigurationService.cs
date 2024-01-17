using Microsoft.Extensions.Configuration;
using service.v1.configuration.Interfaces;

namespace service.v1.configuration
{
    public sealed class ConfigurationService : IEmailConfigurationService, IJWTConfigurationService, 
        IMinioConfigurationService, IFileConfigurationService
    {
        private readonly IConfiguration _cfg;

        public ConfigurationService(IConfiguration cfg) => _cfg = cfg;



        public string GetJWTSecretKey() => _cfg["JWT:SecretKey"];
        public double GetJWTAccessExpireDate() => Convert.ToDouble(_cfg["JWT:AccessExpireDate"]);
        public double GetJWTRefreshExpireDate() => Convert.ToDouble(_cfg["JWT:RefreshExpireDate"]);
        public string GetJWTIssuer() => _cfg["JWT:Issuer"];
        public string GetJWTAudience() => _cfg["JWT:Audience"];



        public string GetMinioEndpoint() => _cfg["Minio:Endpoint"];
        public int GetMinioPort() => Convert.ToInt32(_cfg["Minio:Port"]);
        public string GetMinioAccessKey() => _cfg["Minio:AccessKey"];
        public string GetMinioSecretKey() => _cfg["Minio:SecretKey"];



        public string GetEmailHost() => _cfg["Email:Host"];
        public int GetEmailPort() => Convert.ToInt32(_cfg["Email:Port"]);
        public string GetEmailLogin() => _cfg["Email:Login"];
        public string GetEmailPassword() => _cfg["Email:Password"];



        public string GetDefaultModelsDirectoryPath() => _cfg["File:DefaultModelsParentDirectory"];

        public string GetOtherModelsDirectoryPath() => _cfg["File:OtherModelsParentDirectory"];
        public string GetDefaultModelsUserID() => _cfg["File:DefaultModelsUserID"];
    }
}