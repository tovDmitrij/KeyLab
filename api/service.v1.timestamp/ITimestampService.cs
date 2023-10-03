namespace service.v1.timestamp
{
    public interface ITimestampService
    {
        public double GetUNIXTime(DateTime dateTime);
        public double GetCurrentUNIXTime();
    }
}