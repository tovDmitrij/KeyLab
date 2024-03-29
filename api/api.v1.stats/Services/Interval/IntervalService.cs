using db.v1.stats.DTOs;
using db.v1.stats.Repositories.Interval;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;

namespace api.v1.stats.Services.Interval
{
    public sealed class IntervalService(IIntervalRepository interval, ICacheHelper cache, ICacheConfigurationHelper cacheCfg) : IIntervalService
    {
        private readonly IIntervalRepository _interval = interval;
        private readonly ICacheHelper _cache = cache;
        private readonly ICacheConfigurationHelper _cacheCfg = cacheCfg;

        public List<SelectIntervalDTO> GetIntervals()
        {
            var cacheKey = "intervals";
            if (!_cache.TryGetValue(cacheKey, out List<SelectIntervalDTO>? intervals)) 
            {
                intervals = _interval.SelectIntervals();

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, intervals, minutes);
            }

            return intervals!;
        }
    }
}