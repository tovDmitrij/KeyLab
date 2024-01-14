namespace service.v1.time
{
    public interface ITimeService
    {
        public double GetUNIXTime(DateTime dateTime);
        public double GetCurrentUNIXTime();
        public DateTime GetCurrentDateTimeWithAddedSeconds(double seconds);
    }
}