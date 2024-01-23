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
        public void SetKeyboardsList(Guid key, List<KeyboardDTO> keyboards) => Set(key, keyboards);
        public void DeleteKeyboardsList(Guid userID) => _cache.Remove(userID);



        public bool TryGetFile(Guid fileID, out byte[]? file) => _cache.TryGetValue(fileID, out file);
        public void SetFile(Guid fileID, byte[] file) => Set(fileID, file);
        public void DeleteFile(Guid fileID) => _cache.Remove(fileID);



        private void Set(object key, object value)
        {
            var minutes = GetCacheExpirationTime();

            var expiration = GetExpirationTime(minutes);
            _cache.Set(key, value, expiration);
        }

        private int GetCacheExpirationTime() => _cfg.GetCacheExpirationMinutes();
        private static DateTimeOffset GetExpirationTime(int minutes) => DateTimeOffset.UtcNow.AddMinutes(minutes);
    }
}