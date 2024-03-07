using System.Net;

using Xunit;

namespace misc.unit_tests.API
{
    public sealed class VerificationAPITests : APITest
    {
        [Theory(Skip = "Email spam")]
        [InlineData("dmxikka@gmail.com", HttpStatusCode.OK)]
        public async void PositiveEmailTest(string email, HttpStatusCode excepted)
        {
            var verificationUrl = $"{MainServiceAddress}/verifications/email";
            var response = await PostAsJsonAsync(verificationUrl, new { email });
            var actual = response.StatusCode;

            Assert.True(excepted == actual, $"Произошла ошибка при отправке письма: {email} - {actual}");
        }

        [Theory(Skip = "Email spam")]
        [InlineData("admin@keyboard.ru", HttpStatusCode.BadRequest)]
        public async void NegativeEmailTest(string email, HttpStatusCode excepted)
        {
            var verificationUrl = $"{MainServiceAddress}/verifications/email";
            var response = await PostAsJsonAsync(verificationUrl, new { email });
            var actual = response.StatusCode;

            Assert.True(excepted == actual, $"Произошла ошибка при отправке письма уже зарегистрированному пользователю: {email} - {actual}");
        }

    }
}