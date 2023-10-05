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
        public int GetMinioPort();
        public string GetMinioAccessKey();
        public string GetMinioSecretKey();

        public string GetEmailHost();
        public int GetEmailPort();
        public string GetEmailLogin();
        public string GetEmailPassword();
    }
}