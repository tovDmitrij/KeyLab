namespace helper.v1.time
{
    public interface ITimeHelper
    {
        public double GetCurrentUNIXTime();
        public DateTime GetCurrentDateTimeWithAddedSeconds(double seconds);
    }
}