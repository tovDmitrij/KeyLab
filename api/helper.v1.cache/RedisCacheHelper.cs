using Microsoft.Extensions.Caching.Distributed;

using System.Text.Json;

namespace helper.v1.cache
{
    public sealed class RedisCacheHelper(IDistributedCache cache) : ICacheHelper
    {
        private readonly IDistributedCache _cache = cache;

        public bool TryGetValue<T>(object key, out T? value)
        {
            var json = _cache.GetString(key.ToString()!);

            if (json == null)
            {
                value = default;
                return false;
            }

            value = JsonSerializer.Deserialize<T>(json);
            return true;
        }

        public void DeleteValue(object key) => _cache.Remove(key.ToString()!);

        public void SetValue<T>(object key, T value, int minutes)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(minutes)
            };

            var json = JsonSerializer.Serialize(value);
            _cache.SetString(key.ToString()!, json, options);
        }
    }
}