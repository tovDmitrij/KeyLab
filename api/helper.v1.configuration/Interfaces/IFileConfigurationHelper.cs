namespace helper.v1.configuration.Interfaces
{
    public interface IFileConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public Guid GetDefaultModelsUserID();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetSwitchFilePath(string fileName, string fileExtension);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetSwitchSoundFilePath(string fileName, string fileExtension);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetKeyboardFilePath(Guid userID, string fileName, string fileExtension);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetBoxFilePath(Guid userID, string fileName, string fileExtension);
        /// <exception cref="ArgumentNullException"></exception>
        public string GetKeycapFilePath(Guid userID, Guid kitID, string fileName, string fileExtension);

        /// <exception cref="ArgumentNullException"></exception>
        public string GetModelFilenameExtension();
        /// <exception cref="ArgumentNullException"></exception>
        public string GetPreviewFilenameExtension();
        /// <exception cref="ArgumentNullException"></exception>
        public string GetSoundFilenameExtension();
    }
}