namespace helper.v1.configuration.Interfaces
{
    public interface IEmailConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public string GetEmailHost();
        /// <exception cref="ArgumentNullException"></exception>
        public int GetEmailPort();
        /// <exception cref="ArgumentNullException"></exception>
        public string GetEmailLogin();
        /// <exception cref="ArgumentNullException"></exception>
        public string GetEmailPassword();
    }
}