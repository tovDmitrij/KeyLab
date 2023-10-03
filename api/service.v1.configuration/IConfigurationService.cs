namespace service.v1.configuration
{
    public interface IConfigurationService
    {
        public string GetJWTSecretKey();
        public double GetJWTAccessExpireDate();
        public double GetJWTRefreshExpireDate();
        public string GetJWTIssuer();
        public string GetJWTAudience();

        public string GetMinioEndpoint();
        public string GetMinioPort();
        public string GetMinioAccessKey();
        public string GetMinioSecretKey();
    }
}