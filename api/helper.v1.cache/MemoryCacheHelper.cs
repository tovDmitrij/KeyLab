using Microsoft.Extensions.Caching.Memory;

namespace helper.v1.cache
{
    public sealed class MemoryCacheHelper : ICacheHelper
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheHelper(IMemoryCache cache) => _cache = cache;



        public bool TryGetValue<T>(object key, out T? value) => _cache.TryGetValue(key, out value);

        public void DeleteValue(object key) => _cache.Remove(key);

        public void SetValue<T>(object key, T value, int minutes)
        {
            var expiration = DateTimeOffset.UtcNow.AddMinutes(minutes);
            _cache.Set(key, value, expiration);
        }
    }
}