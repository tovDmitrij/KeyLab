using Microsoft.VisualBasic;

using System.Net;
using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class UserControllerService
    {
        [Fact]
        public void SignIn1()
        {
            var httpClient = new HttpClient();

            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var email = "admin@keyboard.ru";
            var password = "111111112";

            var actual = httpClient.PostAsJsonAsync(signInUrl, new { email, password }).Result.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SignIn2()
        {
            var httpClient = new HttpClient();

            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var email = "admin@keyboard.ru";
            var password = "11111111";

            var actual = httpClient.PostAsJsonAsync(signInUrl, new { email, password }).Result.StatusCode;
            var expected = HttpStatusCode.OK;
            Assert.Equal(expected, actual);
        }
    }
}