namespace helper.v1.time
{
    public sealed class TimeHelper : ITimeHelper
    {
        private readonly DateTime _epoch = DateTime.UnixEpoch;



        public double GetCurrentUNIXTime()
        {
            var currentUNIXTime = DateTime.UtcNow.Subtract(_epoch);
            return currentUNIXTime.TotalSeconds;
        }

        public DateTime GetCurrentDateTimeWithAddedSeconds(double seconds)
        {
            var dateTime = DateTime.UtcNow.AddSeconds(seconds);
            return dateTime;
        }
    }
}