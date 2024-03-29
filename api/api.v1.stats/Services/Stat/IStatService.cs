using api.v1.stats.DTOs.Attendance;
using api.v1.stats.DTOs.Activity;

namespace api.v1.stats.Services.Stat
{
    public interface IStatService
    {
        public List<AttendancePlotDTO> GetAttendanceTimePlot(PostAttendanceStatDTO body);
        public double GetAttendanceTimeAtom(PostAttendanceStatDTO body);

        public List<AttendancePlotDTO> GetAttendanceQuantityPlot(PostAttendanceStatDTO body);
        public double GetAttendanceQuantityAtom(PostAttendanceStatDTO body);

        public List<ActivityPlotDTO> GetActivityTimePlot(PostActivityStatDTO body);
        public double GetActivityTimeAtom(PostActivityStatDTO body);

        public List<ActivityPlotDTO> GetActivityQuantityPlot(PostActivityStatDTO body);
        public double GetActivityQuantityAtom(PostActivityStatDTO body);
    }
}