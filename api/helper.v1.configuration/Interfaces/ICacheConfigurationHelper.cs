namespace helper.v1.configuration.Interfaces
{
    public interface ICacheConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public int GetCacheExpirationMinutes();
    }
}