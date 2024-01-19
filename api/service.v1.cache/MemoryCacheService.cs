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
            var key = GetDefaultKeyboardsListCacheKey();
            var result = _cache.TryGetValue(key, out keyboards);
            return result;
        }

        public void SetDefaultKeyboardsList(List<KeyboardDTO> keyboards)
        {
            var minutes = GetCacheExpirationTime();

            var key = GetDefaultKeyboardsListCacheKey();
            var expiration = GetExpirationTime(minutes);
            _cache.Set(key, keyboards, expiration);
        }



        public bool TryGetKeyboardFile(Guid keyboardID, out byte[]? keyboard) => TryGetFile(keyboardID, out keyboard);

        public void SetKeyboardFile(Guid keyboardID, byte[] keyboard) => SetFile(keyboardID, keyboard);



        private bool TryGetFile(object key, out byte[]? file) => _cache.TryGetValue(key, out file);

        private void SetFile(object key, byte[] file)
        {
            var minutes = GetCacheExpirationTime();

            var expiration = GetExpirationTime(minutes);
            _cache.Set(key, file, expiration);
        }



        private string GetDefaultKeyboardsListCacheKey() => _cfg.GetDefaultKeyboardsListCacheKey();

        private int GetCacheExpirationTime() => _cfg.GetCacheExpirationMinutes();

        private static DateTimeOffset GetExpirationTime(int minutes) => DateTimeOffset.UtcNow.AddMinutes(minutes);
    }
}