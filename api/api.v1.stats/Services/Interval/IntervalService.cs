﻿using component.v1.exceptions;

using db.v1.stats.DTOs;
using db.v1.stats.Repositories.Interval;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.stats.Services.Interval
{
    public sealed class IntervalService : IIntervalService
    {
        private readonly IIntervalRepository _interval;
        private readonly IAdminConfigurationHelper _adminCfg;
        private readonly ILocalizationHelper _localization;
        private readonly ICacheHelper _cache;
        private readonly ICacheConfigurationHelper _cacheCfg;

        public IntervalService(IIntervalRepository interval, IAdminConfigurationHelper cfg, ILocalizationHelper localization,
                               ICacheHelper cache, ICacheConfigurationHelper cacheCfg)
        {
            _interval = interval;
            _adminCfg = cfg;
            _localization = localization;
            _cache = cache;
            _cacheCfg = cacheCfg;
        }



        public List<SelectIntervalDTO> GetIntervals(Guid userID)
        {
            if (userID != _adminCfg.GetDefaultUserID())
                throw new NotAcceptableException(_localization.EndpointIsNotAcceptable());

            if (!_cache.TryGetValue("intervals", out List<SelectIntervalDTO>? intervals)) 
            {
                intervals = _interval.SelectIntervals();

                var minutes = _cacheCfg.GetCacheExpirationMinutes();
                _cache.SetValue("intervals", intervals, minutes);
            }

            return intervals;
        }
    }
}