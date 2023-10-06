using service.v1.timestamp;

using Xunit;

namespace misc.unit_tests
{
    public sealed class TimestampServiceTests
    {
        private readonly TimestampService _service = new();

        private readonly DateTime _epoch = DateTime.UnixEpoch;

        //Часовые пояса?
        [Fact]
        public void GetUNIXTime()
        {
            var utcNow = new DateTime(2018, 08, 18, 4, 22, 16, DateTimeKind.Utc);
            var seconds = 1534566136;
            var result = _service.GetUNIXTime(utcNow);

            Assert.Equal(seconds, result);
        }

        [Fact]
        public void GetCurrentUNIXTime()
        {
            var utcNow = DateTime.UtcNow;
            var seconds = utcNow.Subtract(_epoch).TotalSeconds;
            var result = _service.GetCurrentUNIXTime();

            Assert.Equal((int)seconds, (int)result);
        }
    }
}