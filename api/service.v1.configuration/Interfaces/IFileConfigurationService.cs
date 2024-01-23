namespace service.v1.configuration.Interfaces
{
    public interface IFileConfigurationService
    {
        public string GetModelsParentDirectory();
        public Guid GetDefaultModelsUserID();

        public string GetSwitchModelsDirectory();
        public string GetSwitchSoundsDirectory();
    }
}