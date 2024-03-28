namespace helper.v1.configuration.Interfaces
{
    public interface IAdminConfigurationHelper
    {
        /// <exception cref="ArgumentNullException"></exception>
        public Guid GetDefaultUserID();
    }
}