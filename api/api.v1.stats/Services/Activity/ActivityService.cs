using component.v1.exceptions;

using db.v1.stats.DTOs;
using db.v1.stats.Repositories.Activity;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.stats.Services.Activity
{
    public sealed class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activity;
        private readonly ICacheHelper _cache;
        private readonly ICacheConfigurationHelper _cacheCfg;
        private readonly IAdminConfigurationHelper _adminCfg;
        private readonly ILocalizationHelper _localization;

        public ActivityService(IActivityRepository activity, ICacheHelper cache, ICacheConfigurationHelper cacheCfg, 
                               IAdminConfigurationHelper adminCfg, ILocalizationHelper localization)
        {
            _activity = activity;
            _cache = cache;
            _cacheCfg = cacheCfg;
            _adminCfg = adminCfg;
            _localization = localization;
        }



        public List<SelectActivityDTO> GetActivities(Guid userID)
        {
            if (userID != _adminCfg.GetDefaultUserID())
                throw new NotAcceptableException(_localization.EndpointIsNotAcceptable());

            if (!_cache.TryGetValue("activities", out List<SelectActivityDTO>? activities))
            {
                var notEqualTags = new List<string> { "refresh" };
                activities = _activity.SelectActivities(notEqualTags);

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue("activities", activities, minutes);
            }

            return activities!;
        }
    }
}