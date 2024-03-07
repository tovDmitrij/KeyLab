using System.Net;

using Xunit;

namespace misc.unit_tests.API
{
    public sealed class UserAPITests : APITest
    {
        [Theory]
        [InlineData("admin@keyboard.ru", "11111111", "11111111", "11111111", "11111111", HttpStatusCode.BadRequest)]
        [InlineData("admin@keyboard.ru", "111", "11111111", "11111111", "11111111", HttpStatusCode.BadRequest)]
        [InlineData("admin@keyboard.ru", "11111111", "22222222", "11111111", "11111111", HttpStatusCode.BadRequest)]
        [InlineData("admin@keyboard.ru", "11111111", "11111111", "11111111", "e", HttpStatusCode.BadRequest)]
        [InlineData("admin@keyboard.ru", "11111111", "11111111", "11111111", "1", HttpStatusCode.BadRequest)]
        public async void NegativeSignUpTest(string email, string password, string repeatedPassword, string nickname, 
                                             string emailCode, HttpStatusCode expected)
        {
            var signUpUrl = $"{MainServiceAddress}/users/signUp";
            var response = await PostAsJsonAsync(signUpUrl, new { email, password, repeatedPassword, nickname, emailCode });
            var actual = response.StatusCode;

            Assert.True(actual == expected, $"Провал регистрации был провален: {actual}");

        }



        [Theory]
        [InlineData("admin@keyboard.ru", "11111111", HttpStatusCode.OK)]
        public async void PositiveSignInTest(string email, string password, HttpStatusCode expected)
        {
            var signInUrl = $"{MainServiceAddress}/users/signIn";
            var response = await PostAsJsonAsync(signInUrl, new { email, password });
            var actual = response.StatusCode;

            Assert.True(expected == actual, $"Авторизация не была пройдена: {actual}");
        }

        [Theory]
        [InlineData("admi1@keyboard.ru", "111111111", HttpStatusCode.BadRequest)]
        [InlineData("admin@keyboard.ru", "111111112", HttpStatusCode.BadRequest)]
        public async void NegativeSignInTest(string email, string password, HttpStatusCode expected)
        {
            var signInUrl = $"{MainServiceAddress}/users/signIn";
            var response = await PostAsJsonAsync(signInUrl, new { email, password });
            var actual = response.StatusCode;

            Assert.True(actual == expected, $"Провал авторизации был провален: {actual}");
        }
    }
}