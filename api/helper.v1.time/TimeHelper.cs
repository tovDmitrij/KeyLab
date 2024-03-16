﻿namespace helper.v1.time
{
    public sealed class TimeHelper : ITimeHelper
    {
        private readonly DateTime _epoch = DateTime.UnixEpoch;

        public double GetUNIXTime(DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();

            var UNIXTime = dateTime.Subtract(_epoch);
            return UNIXTime.TotalSeconds;
        }

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