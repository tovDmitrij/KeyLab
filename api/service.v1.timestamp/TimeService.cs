namespace service.v1.timestamp
{
    public sealed class TimeService : ITimeService
    {
        private readonly DateTime EPOCH_TIME = DateTime.UnixEpoch;

        public double GetUNIXTime(DateTime dateTime)
        {
            var UNIXTime = dateTime.Subtract(EPOCH_TIME);
            return UNIXTime.TotalSeconds;
        }

        public double GetCurrentUNIXTime()
        {
            var currentUNIXTime = DateTime.UtcNow.Subtract(EPOCH_TIME);
            return currentUNIXTime.TotalSeconds;
        }

        public DateTime GetDateTimeWithAddedSeconds(double seconds)
        {
            var dateTime = DateTime.UtcNow.AddSeconds(seconds);
            return dateTime;
        }
    }
}