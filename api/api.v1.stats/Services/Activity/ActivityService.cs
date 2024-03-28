using component.v1.activity;
using component.v1.exceptions;

using db.v1.stats.DTOs;
using db.v1.stats.Repositories.Activity;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.stats.Services.Activity
{
    public sealed class ActivityService(IActivityRepository activity, ICacheHelper cache, ICacheConfigurationHelper cacheCfg,
        IAdminConfigurationHelper adminCfg, ILocalizationHelper localization) : IActivityService
    {
        private readonly IActivityRepository _activity = activity;

        private readonly ICacheHelper _cache = cache;
        private readonly ICacheConfigurationHelper _cacheCfg = cacheCfg;
        private readonly IAdminConfigurationHelper _adminCfg = adminCfg;
        private readonly ILocalizationHelper _localization = localization;

        public void AddActivity(ActivityDTO body)
        {

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