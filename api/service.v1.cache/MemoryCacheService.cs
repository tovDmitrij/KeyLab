using Microsoft.Extensions.Caching.Memory;

using service.v1.configuration.Interfaces;

namespace service.v1.cache
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheConfigurationService _cfg;

        public MemoryCacheService(IMemoryCache cache, ICacheConfigurationService cfg)
        {
            _cache = cache;
            _cfg = cfg;
        }



        public bool TryGetValue<T>(object key, out T? value) => _cache.TryGetValue(key, out value);

        public void DeleteValue(object key) => _cache.Remove(key);

        public void SetValue<T>(object key, T value)
        {
            var minutes = _cfg.GetCacheExpirationMinutes();

            var expiration = DateTimeOffset.UtcNow.AddMinutes(minutes);
            _cache.Set(key, value, expiration);
        }
    }
}