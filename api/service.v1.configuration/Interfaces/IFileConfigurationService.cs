namespace service.v1.configuration.Interfaces
{
    public interface IFileConfigurationService
    {
        public string GetDefaultModelsDirectoryPath();
        public string GetOtherModelsDirectoryPath();
    }
}