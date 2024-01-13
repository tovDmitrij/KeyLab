namespace service.v1.configuration.Interfaces
{
    public interface IJWTConfigurationService
    {
        public string GetJWTSecretKey();
        public double GetJWTAccessExpireDate();
        public double GetJWTRefreshExpireDate();
        public string GetJWTIssuer();
        public string GetJWTAudience();
    }
}