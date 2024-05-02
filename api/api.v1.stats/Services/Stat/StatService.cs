using api.v1.stats.DTOs;
using api.v1.stats.DTOs.Attendance;
using api.v1.stats.DTOs.Activity;

using component.v1.exceptions;

using db.v1.stats.DTOs;
using db.v1.stats.Repositories.Activity;
using db.v1.stats.Repositories.History;
using db.v1.stats.Repositories.Interval;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.stats.Services.Stat
{
    public sealed class StatService(IIntervalRepository interval, IHistoryRepository history,
        ICacheHelper cache, ILocalizationHelper localization, IStatConfigurationHelper statCfg, ICacheConfigurationHelper cacheCfg,
        IActivityRepository activity) : IStatService
    {
        private readonly IIntervalRepository _interval = interval;
        private readonly IActivityRepository _activity = activity;
        private readonly IHistoryRepository _history = history;

        private readonly ICacheHelper _cache = cache;
        private readonly ILocalizationHelper _localization = localization;
        private readonly IStatConfigurationHelper _statCfg = statCfg;
        private readonly ICacheConfigurationHelper _cacheCfg = cacheCfg;

        public List<AttendanceTimePlotDTO> GetAttendanceTimePlot(PostAttendanceStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<AttendanceTimePlotDTO>();
            foreach (var period in periods)
            {
                try
                {
                    var cacheKey = _cacheCfg.GetAttendanceTimeCacheKey(period.LeftDate, period.RightDate);
                    if (!_cache.TryGetValue(cacheKey, out List<SelectHistoryDTO>? activities))
                    {
                        activities = _history.SelectHistories(period.LeftDate, period.RightDate)
                            .OrderBy(x => x.UserID).ThenBy(x => x.Date).ToList();

                        var minutes = _cacheCfg.GetCacheExpirationMinutes();
                        _cache.SetValue(cacheKey, activities, minutes);
                    }

                    var userIDs = activities!.Select(x => x.UserID).Distinct();
                    var times = new List<double>();
                    foreach (var userID in userIDs)
                    {
                        var currentUserActivities = activities!.Where(x => x.UserID == userID);
                        var aliveTime = currentUserActivities.Last().Date - currentUserActivities.First().Date;
                        if (aliveTime > _statCfg.GetStatisticAliveTimeSeconds())
                        {
                            times.Add(aliveTime);
                        }
                    }
                    plotData.Add(new(period.LeftDate, period.RightDate, times.Average()));
                }
                catch
                {
                    plotData.Add(new(period.LeftDate, period.RightDate, 0.0));
                }
            }
            return plotData;
        }

        public double GetAttendanceTimeAtom(PostAttendanceStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<double>();
            foreach (var period in periods)
            {
                try
                {
                    var cacheKey = _cacheCfg.GetAttendanceTimeCacheKey(period.LeftDate, period.RightDate);
                    if (!_cache.TryGetValue(cacheKey, out List<SelectHistoryDTO>? activities))
                    {
                        activities = _history.SelectHistories(period.LeftDate, period.RightDate)
                            .OrderBy(x => x.UserID).ThenBy(x => x.Date).ToList();

                        var minutes = _cacheCfg.GetCacheExpirationMinutes();
                        _cache.SetValue(cacheKey, activities, minutes);
                    }

                    var userIDs = activities!.Select(x => x.UserID).Distinct();
                    var times = new List<double>();
                    foreach (var userID in userIDs)
                    {
                        var currentUserActivities = activities!.Where(x => x.UserID == userID);
                        var aliveTime = currentUserActivities.Last().Date - currentUserActivities.First().Date;
                        if (aliveTime > _statCfg.GetStatisticAliveTimeSeconds())
                        {
                            times.Add(aliveTime);
                        }
                    }
                    plotData.Add(times.Average());
                }
                catch
                {
                    plotData.Add(0.0);
                }
            }
            return plotData.Average();
        }



        public List<AttendanceQuantityPlotDTO> GetAttendanceQuantityPlot(PostAttendanceStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<AttendanceQuantityPlotDTO>();
            foreach (var period in periods)
            {
                var cacheKey = _cacheCfg.GetAttendanceQuantityCacheKey(period.LeftDate, period.RightDate);
                if (!_cache.TryGetValue(cacheKey, out int count))
                {
                    count = _history.SelectCountOfDistinctUserID(period.LeftDate, period.RightDate);

                    var minutes = _cacheCfg.GetCacheExpirationMinutes();
                    _cache.SetValue(cacheKey, count, minutes);
                }
                plotData.Add(new(period.LeftDate, period.RightDate, count));
            }
            return plotData;
        }

        public double GetAttendanceQuantityAtom(PostAttendanceStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<int>();
            foreach (var period in periods)
            {
                var cacheKey = _cacheCfg.GetAttendanceQuantityCacheKey(period.LeftDate, period.RightDate);
                if (!_cache.TryGetValue(cacheKey, out int count))
                {
                    count = _history.SelectCountOfDistinctUserID(period.LeftDate, period.RightDate);

                    var minutes = _cacheCfg.GetCacheExpirationMinutes();
                    _cache.SetValue(cacheKey, count, minutes);
                }
                plotData.Add(count);
            }
            return plotData.Average();
        }



        public List<ActivityPlotDTO> GetActivityTimePlot(PostActivityStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<ActivityPlotDTO>();
            foreach (var period in periods)
            {
                var activityTimes = new Dictionary<Guid, double>();
                foreach (var activityID in body.ActivityIDs)
                {
                    try
                    {
                        var cacheKey = _cacheCfg.GetActivityTimeCacheKey(period.LeftDate, period.RightDate);
                        if (!_cache.TryGetValue(cacheKey, out List<SelectHistoryDTO>? activities))
                        {
                            activities = _history.SelectHistories(period.LeftDate, period.RightDate, activityID)
                                .OrderBy(x => x.UserID).ThenBy(x => x.Date).ToList();

                            var minutes = _cacheCfg.GetCacheExpirationMinutes();
                            _cache.SetValue(cacheKey, activities, minutes);
                        }

                        var userIDs = activities!.Select(x => x.UserID).Distinct();
                        var times = new List<double>();
                        foreach (var userID in userIDs)
                        {
                            var currentUserActivities = activities!.Where(x => x.UserID == userID);
                            var aliveTime = currentUserActivities.Last().Date - currentUserActivities.First().Date;
                            if (aliveTime > _statCfg.GetStatisticAliveTimeSeconds())
                            {
                                times.Add(aliveTime);
                            }
                        }
                        activityTimes.Add(activityID, times.Average());
                    }
                    catch
                    {
                        activityTimes.Add(activityID, 0);
                    }
                }
                plotData.Add(new(period.LeftDate, period.RightDate, activityTimes));
            }
            return plotData;
        }

        public double GetActivityTimeAtom(PostActivityStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<double>();
            foreach (var period in periods)
            {
                var activityTimes = new List<double>();
                foreach (var activityID in body.ActivityIDs)
                {
                    try
                    {
                        var cacheKey = _cacheCfg.GetActivityTimeCacheKey(period.LeftDate, period.RightDate);
                        if (!_cache.TryGetValue(cacheKey, out List<SelectHistoryDTO>? activities))
                        {
                            activities = _history.SelectHistories(period.LeftDate, period.RightDate, activityID)
                                .OrderBy(x => x.UserID).ThenBy(x => x.Date).ToList();

                            var minutes = _cacheCfg.GetCacheExpirationMinutes();
                            _cache.SetValue(cacheKey, activities, minutes);
                        }

                        var userIDs = activities!.Select(x => x.UserID).Distinct();
                        var times = new List<double>();
                        foreach (var userID in userIDs)
                        {
                            var currentUserActivities = activities!.Where(x => x.UserID == userID);
                            var aliveTime = currentUserActivities.Last().Date - currentUserActivities.First().Date;
                            if (aliveTime > _statCfg.GetStatisticAliveTimeSeconds())
                            {
                                times.Add(aliveTime);
                            }
                        }
                        activityTimes.Add(times.Average());
                    }
                    catch
                    {
                        activityTimes.Add(0);
                    }
                }
                plotData.Add(activityTimes.Average());
            }
            return plotData.Average();
        }



        public List<ActivityPlotDTO> GetActivityQuantityPlot(PostActivityStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<ActivityPlotDTO>();
            foreach (var period in periods)
            {
                var quantities = new Dictionary<Guid, double>();
                foreach (var activityID in body.ActivityIDs)
                {
                    var cacheKey = _cacheCfg.GetActivityQuantityCacheKey(period.LeftDate, period.RightDate);
                    if (!_cache.TryGetValue(cacheKey, out int count))
                    {
                        count = _history.SelectCountOfDistinctUserID(period.LeftDate, period.RightDate, activityID);

                        var minutes = _cacheCfg.GetCacheExpirationMinutes();
                        _cache.SetValue(cacheKey, count, minutes);
                    }
                    quantities.Add(activityID, count);

                }
                plotData.Add(new(period.LeftDate, period.RightDate, quantities));
            }
            return plotData;
        }

        public double GetActivityQuantityAtom(PostActivityStatDTO body)
        {
            ValidateBody(body);
            var periods = GetPeriods(body.LeftDate, body.RightDate, body.IntervalID);

            var plotData = new List<double>();
            foreach (var period in periods)
            {
                var quantities = new List<int>();
                foreach (var activityID in body.ActivityIDs)
                {
                    var cacheKey = _cacheCfg.GetActivityQuantityCacheKey(period.LeftDate, period.RightDate);
                    if (!_cache.TryGetValue(cacheKey, out int count))
                    {
                        count = _history.SelectCountOfDistinctUserID(period.LeftDate, period.RightDate, activityID);

                        var minutes = _cacheCfg.GetCacheExpirationMinutes();
                        _cache.SetValue(cacheKey, count, minutes);
                    }
                    quantities.Add(count);
                }
                plotData.Add(quantities.Average());
            }
            return plotData.Average();
        }



        private void ValidateBody(PostAttendanceStatDTO body)
        {
            ValidateDates(body.LeftDate, body.RightDate);
            ValidateIntervalID(body.IntervalID);
        }

        private void ValidateBody(PostActivityStatDTO body)
        {
            
            ValidateDates(body.LeftDate, body.RightDate);
            ValidateIntervalID(body.IntervalID);
            foreach (var pageID in body.ActivityIDs)
            {
                ValidateActivityID(pageID);
            }
        }

        private List<PeriodDTO> GetPeriods(double leftDate, double rightDate, Guid intervalID)
        {
            var intervalSeconds = _interval.SelectIntervalSeconds(intervalID);

            var periodSeconds = rightDate - leftDate;
            var remain = periodSeconds % intervalSeconds;
            var shiftedLeftDate = leftDate;
            if (remain != 0)
            {
                shiftedLeftDate = leftDate - intervalSeconds + remain;
            }

            var periods = new List<PeriodDTO>();
            for (var i = shiftedLeftDate; i < rightDate; i += intervalSeconds)
            {
                periods.Add(new(i, i + intervalSeconds));
            }
            return periods;
        }



        private void ValidateDates(double leftDate, double rightDate)
        {
            if (leftDate > rightDate)
                throw new BadRequestException(_localization.StatsLeftDateGreaterThanRightDate());
        }

        private void ValidateIntervalID(Guid intervalID)
        {
            if (!_interval.IsIntervalExist(intervalID))
                throw new BadRequestException(_localization.IntervalIsNotExist());
        }

        private void ValidateActivityID(Guid activityID)
        {
            if (!_activity.IsActivityExistByID(activityID))
                throw new BadRequestException(_localization.ActivityIsNotExist());
        }
    }
}