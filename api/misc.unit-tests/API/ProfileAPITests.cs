using misc.unit_tests.Responses;

using System.Net.Http.Headers;

using Xunit;

namespace misc.unit_tests.API
{
    public sealed class ProfileAPITests : APITest
    {
        [Theory]
        [InlineData("admin@keyboard.ru", "11111111", 0)]
        public async void PositiveNicknameTest(string email, string password, int excepted)
        {
            var signInUrl = $"{MainServiceAddress}/users/signIn";
            var signInResponse = await PostAsJsonAsync(signInUrl, new { email, password });

            var profileUrl = $"{MainServiceAddress}/profiles/nickname";
            using var request = new HttpRequestMessage(HttpMethod.Get, profileUrl);

            var signInJSON = await ReadFromJsonAsync<AccessToken>(signInResponse);
            var accessToken = signInJSON.accessToken;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var profileResponse = await GetAsync(request);
            var json = await ReadAsStringAsync(profileResponse);
            var actual = json.Length;

            Assert.True(actual != excepted, $"Длина никнейма администратора равна нулю");
        }
    }
}