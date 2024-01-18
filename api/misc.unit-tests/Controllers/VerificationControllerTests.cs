using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class VerificationControllerTests
    {
        [Fact(Skip = "Email spam")]
        public void VerificationEmail1()
        {
            var httpClient = new HttpClient();

            var verificationUrl = "http://127.0.0.1:6005/api/v1/verifications/email"; //Docker!
            var email = "dmxikka@gmail.com";

            var actual = httpClient.PostAsJsonAsync(verificationUrl, new { email }).Result.StatusCode;
            var excepted = HttpStatusCode.OK;
            Assert.Equal(excepted, actual);
        }

        [Fact(Skip = "Email spam")]
        public void VerificationEmail2()
        {
            var httpClient = new HttpClient();

            var verificationUrl = "http://127.0.0.1:6005/api/v1/verifications/email"; //Docker!
            var email = "admin@keyboard.ru";

            var actual = httpClient.PostAsJsonAsync(verificationUrl, new { email }).Result.StatusCode;
            var excepted = HttpStatusCode.BadRequest;
            Assert.Equal(excepted, actual);
        }

    }
}