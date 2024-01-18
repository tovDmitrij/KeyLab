using service.v1.time;

using Xunit;

namespace misc.unit_tests.Services
{
    public sealed class TimeServiceTests
    {
        [Fact]
        public void GetUNIXTime()
        {
            var time = new TimeService();
            var data = new DateTime(2024, 1, 13, 23, 40, 26); //DateTime будет конвертироваться в UTC

            var excepted = 1705178426;
            var actual = time.GetUNIXTime(data);

            Assert.Equal(excepted, actual);
        }

        [Fact]
        public void GetCurrentUNIXTime()
        {
            var time = new TimeService();
            var epoch = DateTime.UnixEpoch;

            var excepted = DateTime.UtcNow.Subtract(epoch).TotalSeconds;
            var actual = time.GetCurrentUNIXTime();

            var tolerance = 0.1;
            Assert.Equal(excepted, actual, tolerance);
        }

        [Fact]
        public void GetCurrentDateTimeWithAddedSeconds()
        {
            var time = new TimeService();
            var seconds = 146;

            var excepted = DateTime.UtcNow.AddSeconds(seconds);
            var actual = time.GetCurrentDateTimeWithAddedSeconds(seconds);

            var presicion = TimeSpan.FromSeconds(1);
            Assert.Equal(excepted, actual, presicion);
        }
    }
}