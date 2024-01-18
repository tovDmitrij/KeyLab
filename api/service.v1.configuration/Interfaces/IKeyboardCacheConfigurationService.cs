namespace service.v1.configuration.Interfaces
{
    public interface IKeyboardCacheConfigurationService
    {
        public int GetCacheExpirationMinutes();
        public string GetDefaultKeyboardsListCacheKey();
    }
}