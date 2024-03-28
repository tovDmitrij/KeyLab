using api.v1.stats.DTOs;
using api.v1.stats.DTOs.Attendance;

using component.v1.exceptions;

using db.v1.stats.Repositories.History;
using db.v1.stats.Repositories.Interval;

using helper.v1.cache;
using helper.v1.configuration.Interfaces;
using helper.v1.localization.Helper;

namespace api.v1.stats.Services.Stat
{
    public sealed class StatService(IIntervalRepository interval, IAdminConfigurationHelper adminCfg, IHistoryRepository history,
        ICacheHelper cache, ILocalizationHelper localization) : IStatService
    {
        private readonly IIntervalRepository _interval = interval;
        private readonly IAdminConfigurationHelper _adminCfg = adminCfg;
        private readonly IHistoryRepository _history = history;
        private readonly ICacheHelper _cache = cache;
        private readonly ILocalizationHelper _localization = localization;

        public List<AttendancePlotDTO> GetAttendanceQuantityPlot(PostAttendanceStatDTO body, Guid userID)
        {
            var periods = ValidateBodyAndGetPeriods(body, userID);

            var quantityPlotData = new List<AttendancePlotDTO>();
            foreach (var period in periods)
            {
                var count = _history.SelectDistinctCountOfUserIDByPeriod(period.LeftDate, period.RightDate);
                quantityPlotData.Add(new(period.LeftDate, period.RightDate, count));
            }
            return quantityPlotData;
        }

        public double GetAttendanceQuantityAtom(PostAttendanceStatDTO body, Guid userID)
        {
            var periods = ValidateBodyAndGetPeriods(body, userID);

            var quantityPlotData = new List<int>();
            foreach (var period in periods)
            {
                var count = _history.SelectDistinctCountOfUserIDByPeriod(period.LeftDate, period.RightDate);
                quantityPlotData.Add(count);
            }
            return quantityPlotData.Average();
        }



        private List<PeriodDTO> ValidateBodyAndGetPeriods(PostAttendanceStatDTO body, Guid userID)
        {
            ValidateUserID(userID);
            ValidateDates(body.LeftDate, body.RightDate);
            ValidateIntervalID(body.IntervalID);

            var intervalSeconds = _interval.SelectIntervalSeconds(body.IntervalID);

            var periodSeconds = body.RightDate - body.LeftDate;
            var remain = periodSeconds % intervalSeconds;
            var shiftedLeftDate = body.LeftDate;
            if (remain != 0)
            {
                shiftedLeftDate = body.LeftDate - intervalSeconds + remain;
            }

            var periods = new List<PeriodDTO>();
            for (var i = shiftedLeftDate; i < body.RightDate; i += intervalSeconds)
            {
                periods.Add(new(i, i + intervalSeconds));
            }
            return periods;
        }



        private void ValidateUserID(Guid userID)
        {
            if (userID != _adminCfg.GetDefaultUserID())
                throw new BadRequestException(_localization.EndpointIsNotAcceptable());
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
    }
}