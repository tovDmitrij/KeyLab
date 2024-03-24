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
        public string GetKeyboardModelFilePath(Guid userID, string fileName);

        /// <exception cref="ArgumentNullException"></exception>
        public string GetBoxModelFilePath(Guid userID, string fileName);



        /// <exception cref="ArgumentNullException"></exception>
        public string GetErrorImageFilePath();
    }
}