using api.v1.stats.DTOs.Attendance;
using api.v1.stats.DTOs.Activity;

namespace api.v1.stats.Services.Stat
{
    public interface IStatService
    {
        public List<AttendanceTimePlotDTO> GetAttendanceTimePlot(PostAttendanceStatDTO body);
        public double GetAttendanceTimeAtom(PostAttendanceStatDTO body);

        public List<AttendanceQuantityPlotDTO> GetAttendanceQuantityPlot(PostAttendanceStatDTO body);
        public int GetAttendanceQuantityAtom(PostAttendanceStatDTO body);

        public List<ActivityTimePlotDTO> GetActivityTimePlot(PostActivityStatDTO body);
        public double GetActivityTimeAtom(PostActivityStatDTO body);

        public List<ActivityQuantityPlotDTO> GetActivityQuantityPlot(PostActivityStatDTO body);
        public int GetActivityQuantityAtom(PostActivityStatDTO body);
    }
}