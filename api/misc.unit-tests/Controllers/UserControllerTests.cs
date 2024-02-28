using System.Net;
using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class UserControllerTests
    {
        [Fact]
        public async void SignIn_400_email()
        {
            var httpClient = new HttpClient();

            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var email = "admin@keyboar.ru";
            var password = "111111111";

            var response = await httpClient.PostAsJsonAsync(signInUrl, new { email, password });

            var actual = response.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void SignIn_400_password()
        {
            var httpClient = new HttpClient();

            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var email = "admin@keyboard.ru";
            var password = "111111112";

            var response = await httpClient.PostAsJsonAsync(signInUrl, new { email, password });

            var actual = response.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void SignIn_200()
        {
            var httpClient = new HttpClient();

            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var email = "admin@keyboard.ru";
            var password = "11111111";

            var response = await httpClient.PostAsJsonAsync(signInUrl, new { email, password });

            var actual = response.StatusCode;
            var expected = HttpStatusCode.OK;
            Assert.Equal(expected, actual);
        }
    }
}