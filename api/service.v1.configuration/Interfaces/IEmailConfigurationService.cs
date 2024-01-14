namespace service.v1.configuration.Interfaces
{
    public interface IEmailConfigurationService
    {
        public string GetEmailHost();
        public int GetEmailPort();
        public string GetEmailLogin();
        public string GetEmailPassword();
    }
}