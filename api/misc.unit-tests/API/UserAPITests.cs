using misc.unit_tests.API;

using System.Net;
using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class UserAPITests : APITest
    {
        [Fact]
        public async void SignIn_400_email()
        {
            var email = "admin@keyboar.ru";
            var password = "111111111";
            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var response = await PostAsJsonAsync(signInUrl, new { email, password });

            var actual = response.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.True(actual == expected, $"Провал авторизации из-за почты был провален: {ReadAsStringAsync(response)}");
        }

        [Fact]
        public async void SignIn_400_password()
        {
            var email = "admin@keyboard.ru";
            var password = "111111112";
            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var response = await PostAsJsonAsync(signInUrl, new { email, password });

            var actual = response.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.True(actual == expected, $"Провал авторизации из-за пароля был провален: {ReadAsStringAsync(response)}");
        }

        [Fact]
        public async void SignIn_200()
        {
            var email = "admin@keyboard.ru";
            var password = "11111111";
            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var response = await PostAsJsonAsync(signInUrl, new { email, password });

            var actual = response.StatusCode;
            var expected = HttpStatusCode.OK;
            Assert.True(expected == actual, $"Авторизация не была пройдена: {ReadAsStringAsync(response)}");
        }
    }
}