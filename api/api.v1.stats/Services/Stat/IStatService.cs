using api.v1.stats.DTOs.Attendance;
using api.v1.stats.DTOs.Page;

namespace api.v1.stats.Services.Stat
{
    public interface IStatService
    {
        public List<AttendancePlotDTO> GetAttendanceTimePlot(PostAttendanceStatDTO body, Guid userID);
        public double GetAttendanceTimeAtom(PostAttendanceStatDTO body, Guid userID);

        public List<AttendancePlotDTO> GetAttendanceQuantityPlot(PostAttendanceStatDTO body, Guid userID);
        public double GetAttendanceQuantityAtom(PostAttendanceStatDTO body, Guid userID);

        public List<PagePlotDTO> GetPageTimePlot(PostPageStatDTO body, Guid userID);
        public double GetPageTimeAtom(PostPageStatDTO body, Guid userID);

        public List<PagePlotDTO> GetPageQuantityPlot(PostPageStatDTO body, Guid userID);
        public double GetPageQuantityAtom(PostPageStatDTO body, Guid userID);
    }
}