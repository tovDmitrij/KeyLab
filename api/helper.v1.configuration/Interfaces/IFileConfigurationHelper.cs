namespace helper.v1.configuration.Interfaces
{
    public interface IFileConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public Guid GetDefaultModelsUserID();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSwitchFilePath(string fileName);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetSwitchSoundFilePath(string fileName);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetKeyboardFilePath(Guid userID, string fileName);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetBoxFilePath(Guid userID, string fileName);
    }
}