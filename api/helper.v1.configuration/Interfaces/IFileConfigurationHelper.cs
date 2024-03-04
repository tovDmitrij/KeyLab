namespace helper.v1.configuration.Interfaces
{
    public interface IFileConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public string GetModelsParentDirectory();

        /// <exception cref="ArgumentNullException"></exception>
        public Guid GetDefaultModelsUserID();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSwitchModelsDirectory();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSwitchSoundsDirectory();
    }
}