namespace helper.v1.configuration.Interfaces
{
    public interface IMinioConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public string GetMinioEndpoint();

        /// <exception cref="ArgumentNullException"></exception>
        public int GetMinioPort();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetMinioAccessKey();

        /// <exception cref="ArgumentNullException"></exception>
        public string GetMinioSecretKey();
    }
}