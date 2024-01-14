namespace service.v1.configuration.Interfaces
{
    public interface IMinioConfigurationService
    {
        public string GetMinioEndpoint();
        public int GetMinioPort();
        public string GetMinioAccessKey();
        public string GetMinioSecretKey();
    }
}