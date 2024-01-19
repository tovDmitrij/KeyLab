using db.v1.main.DTOs;

using Microsoft.Extensions.Caching.Memory;

using service.v1.configuration.Interfaces;

namespace service.v1.cache
{
    public sealed class MemoryCacheService : IKeyboardCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheConfigurationService _cfg;

        public MemoryCacheService(IMemoryCache cache, ICacheConfigurationService cfg)
        {
            _cache = cache;
            _cfg = cfg;
        }



        public bool TryGetKeyboardsList(Guid key, out List<KeyboardDTO>? keyboards) => _cache.TryGetValue(key, out keyboards);

        public void SetKeyboardsList(Guid key, List<KeyboardDTO> keyboards)
        {
            var minutes = GetCacheExpirationTime();

            var expiration = GetExpirationTime(minutes);
            _cache.Set(key, keyboards, expiration);
        }



        public bool TryGetFile(Guid key, out byte[]? file) => _cache.TryGetValue(key, out file);

        public void SetFile(Guid key, byte[] file)
        {
            var minutes = GetCacheExpirationTime();

            var expiration = GetExpirationTime(minutes);
            _cache.Set(key, file, expiration);
        }




        private int GetCacheExpirationTime() => _cfg.GetCacheExpirationMinutes();

        private static DateTimeOffset GetExpirationTime(int minutes) => DateTimeOffset.UtcNow.AddMinutes(minutes);
    }
}