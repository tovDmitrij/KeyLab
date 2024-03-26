namespace helper.v1.configuration.Interfaces
{
    public interface IJWTConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public string GetJWTSecretKey();
        /// <exception cref="ArgumentNullException"></exception>
        public double GetJWTAccessExpireDate();
        /// <exception cref="ArgumentNullException"></exception>
        public double GetJWTRefreshExpireDate();
        /// <exception cref="ArgumentNullException"></exception>
        public string GetJWTIssuer();
        /// <exception cref="ArgumentNullException"></exception>
        public string GetJWTAudience();
    }
}