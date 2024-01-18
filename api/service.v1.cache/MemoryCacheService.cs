using db.v1.main.DTOs;

using Microsoft.Extensions.Caching.Memory;

using service.v1.configuration.Interfaces;

namespace service.v1.cache
{
    public sealed class MemoryCacheService : IKeyboardCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IKeyboardCacheConfigurationService _cfg;

        public MemoryCacheService(IMemoryCache cache, IKeyboardCacheConfigurationService cfg)
        {
            _cache = cache;
            _cfg = cfg;
        }



        public bool TryGetDefaultKeyboardsList(out List<KeyboardDTO>? keyboards)
        {
            var key = _cfg.GetDefaultKeyboardsListCacheKey();
            var result = _cache.TryGetValue(key, out keyboards);
            return result;
        }

        public void SetDefaultKeyboardsList(List<KeyboardDTO> keyboards)
        {
            var minutes = _cfg.GetCacheExpirationMinutes();

            var key = _cfg.GetDefaultKeyboardsListCacheKey();
            var expiration = DateTimeOffset.UtcNow.AddMinutes(minutes);
            _cache.Set(key, keyboards, expiration);
        }
    }
}