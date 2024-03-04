namespace helper.v1.time
{
    public interface ITimeHelper
    {
        public double GetUNIXTime(DateTime dateTime);
        public double GetCurrentUNIXTime();
        public DateTime GetCurrentDateTimeWithAddedSeconds(double seconds);
    }
}