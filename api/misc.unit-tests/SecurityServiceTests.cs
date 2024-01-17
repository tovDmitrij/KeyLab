using service.v1.security.Service;
using service.v1.time;

using Xunit;

namespace misc.unit_tests
{
    public sealed class SecurityServiceTests
    {
        [Fact]
        public void GenerateRandomValue()
        {
            var security = new SecurityService(new TimeService());

            var actual = security.GenerateRandomValue();

            Assert.InRange(actual.Length, 32, 64);
        }

        [Fact]
        public void HashPassword()
        {
            var security = new SecurityService(new TimeService());

            var salt = "111";
            var password = "222";

            var actual = security.HashPassword(password, salt);

            Assert.InRange(actual.Length, 64, 256);
        }

        [Fact]
        public void GenerateEmailVerificationCodeLength()
        {
            var security = new SecurityService(new TimeService());

            var actual = security.GenerateEmailVerificationCode();

            Assert.InRange(actual.Value.Length, 6, 6);
        }

        [Fact]
        public void GenerateEmailVerificationCodeExpireDate()
        {
            var time = new TimeService();
            var security = new SecurityService(time);

            var actual = security.GenerateEmailVerificationCode();

            var low = time.GetCurrentUNIXTime();
            var high = low + 302;
            Assert.InRange(actual.ExpireDate, low, high);
        }
    }
}