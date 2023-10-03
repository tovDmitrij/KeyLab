using Microsoft.Extensions.Configuration;

namespace service.v1.configuration
{
    public sealed class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _cfg;

        public ConfigurationService(IConfiguration cfg) => _cfg = cfg;



        public string GetJWTSecretKey() => _cfg["JWT:SecretKey"]!;
        public double GetJWTAccessExpireDate() => Convert.ToDouble(_cfg["JWT:AccessExpireDate"]);
        public double GetJWTRefreshExpireDate() => Convert.ToDouble(_cfg["JWT:RefreshExpireDate"]);
        public string GetJWTIssuer() => _cfg["JWT:Issuer"]!;
        public string GetJWTAudience() => _cfg["JWT:Audience"]!;



        public string GetMinioEndpoint() => _cfg["Minio:Endpoint"]!;
        public string GetMinioPort() => _cfg["Minio:Port"]!;
        public string GetMinioAccessKey() => _cfg["Minio:AccessKey"]!;
        public string GetMinioSecretKey() => _cfg["Minio:SecretKey"]!;
    }
}