using db.v1.stats.DTOs;
using db.v1.stats.Repositories.Activity;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;

namespace api.v1.stats.Services.Activity
{
    public sealed class ActivityService(IActivityRepository activity, ICacheHelper cache, ICacheConfigurationHelper cacheCfg) : IActivityService
    {
        private readonly IActivityRepository _activity = activity;

        private readonly ICacheHelper _cache = cache;
        private readonly ICacheConfigurationHelper _cacheCfg = cacheCfg;

        public List<SelectActivityDTO> GetActivities()
        {
            var cacheKey = "activities";
            if (!_cache.TryGetValue(cacheKey, out List<SelectActivityDTO>? activities))
            {
                activities = _activity.SelectActivities();

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue(cacheKey, activities, minutes);
            }

            return activities!;
        }
    }
}