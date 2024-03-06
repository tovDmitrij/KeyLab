using misc.unit_tests.API;

using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class VerificationAPITests : APITest
    {
        [Fact(Skip = "Email spam")]
        public async void VerificationEmail_200()
        {
            var email = "dmxikka@gmail.com";
            var verificationUrl = "http://127.0.0.1:6005/api/v1/verifications/email"; //Docker!
            var response = await PostAsJsonAsync(verificationUrl, new { email });

            var actual = response.StatusCode;
            var excepted = HttpStatusCode.OK;
            Assert.Equal(excepted, actual);
        }

        [Fact(Skip = "Email spam")]
        public async void VerificationEmail_400()
        {
            var httpClient = new HttpClient();

            var verificationUrl = "http://127.0.0.1:6005/api/v1/verifications/email"; //Docker!
            var email = "admin@keyboard.ru";

            var response = await httpClient.PostAsJsonAsync(verificationUrl, new { email });

            var actual = response.StatusCode;
            var excepted = HttpStatusCode.BadRequest;
            Assert.Equal(excepted, actual);
        }

    }
}