namespace api.v1.stats.DTOs.Attendance
{
    public sealed record PostAttendanceStatDTO(double LeftDate, double RightDate, Guid IntervalID);
}