using api.v1.stats.DTOs.Attendance;

namespace api.v1.stats.Services.Stat
{
    public interface IStatService
    {
        public List<AttendancePlotDTO> GetAttendanceQuantityPlot(PostAttendanceStatDTO body, Guid userID);
        public double GetAttendanceQuantityAtom(PostAttendanceStatDTO body, Guid userID);
    }
}