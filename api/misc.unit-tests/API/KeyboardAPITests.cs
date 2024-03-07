using db.v1.main.DTOs.Keyboard;

using misc.unit_tests.Responses;

using System.Net;
using System.Net.Http.Headers;

using Xunit;

namespace misc.unit_tests.API
{
    public sealed class KeyboardAPITests : APITest
    {
        [Theory]
        [InlineData(0)]
        public async void PositiveDefaultKeyboardsTest(int excepted)
        {
            var keyboardUrl = $"{MainServiceAddress}/keyboards/default";
            var response = await GetAsync(keyboardUrl);
            var json = await ReadFromJsonAsync<List<SelectKeyboardDTO>>(response);
            var actual = json.Count;

            Assert.True(actual != excepted, $"Список клавиатур по умолчанию пустой: {actual}");
        }

        [Theory]
        [InlineData("d296c943-4894-484a-b0c3-9b3783accbaa", 1_048_576)]
        public async void PositiveKeyboardFileTest(Guid keyboardID, int excepted)
        {
            var keyboardUrl = $"{MainServiceAddress}/keyboards?keyboardID={keyboardID}";
            var response = await GetAsync(keyboardUrl);
            var json = await ReadAsStringAsync(response);
            var actual = json.Length;

            Assert.True(actual > excepted, $"Размер файла меньше 1 мегабайта: {actual}");
        }

        [Theory]
        [InlineData("d296c943-4894-484a-b0c3-9b3783accba2", HttpStatusCode.BadRequest)]
        public async void NegativeKeyboardFileTest(Guid keyboardID, HttpStatusCode excepted)
        {
            var keyboardUrl = $"{MainServiceAddress}/keyboards?keyboardID={keyboardID}";
            var response = await GetAsync(keyboardUrl);
            var actual = response.StatusCode;

            Assert.True(actual == excepted, $"Файл с несуществующим идентификатором был получен: {keyboardID}");
        }

        [Theory]
        [InlineData("admin@keyboard.ru", "11111111", 0)]
        public async void PositiveUserKeyboardsTest(string email, string password, int excepted)
        {
            var signInUrl = $"{MainServiceAddress}/users/signIn";
            var signInResponse = await PostAsJsonAsync(signInUrl, new { email, password });

            var keyboardUrl = $"{MainServiceAddress}/keyboards/auth";
            using var request = new HttpRequestMessage(HttpMethod.Get, keyboardUrl);

            var signInJSON = await ReadFromJsonAsync<AccessToken>(signInResponse);
            var accessToken = signInJSON.accessToken;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var keyboardResponse = await GetAsync(request);
            var keyboardJSON = await ReadFromJsonAsync<List<SelectKeyboardDTO>>(keyboardResponse);
            var actual = keyboardJSON.Count;

            Assert.True(actual > excepted, $"Список клавиатур администратора равно нулю: {password}");
        }
    }
}