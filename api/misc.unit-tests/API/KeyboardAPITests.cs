using db.v1.main.DTOs.Keyboard;

using misc.unit_tests.API;
using misc.unit_tests.Responses;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class KeyboardAPITests : APITest
    {
        [Fact]
        public async void GetDefaultKeyboardsList_200()
        {
            var keyboardUrl = "http://127.0.0.1:6005/api/v1/keyboards/default";
            var response = await GetAsync(keyboardUrl);

            var actual = ReadFromJsonAsync<List<SelectKeyboardDTO>>(response).Count;
            Assert.True(actual != 0, $"Список клавиатур по умолчанию пустой: {actual}");
        }

        [Fact]
        public async void GetKeyboardFile_200()
        {
            var keyboardID = Guid.Parse("d296c943-4894-484a-b0c3-9b3783accbaa");
            var keyboardUrl = $"http://127.0.0.1:6005/api/v1/keyboards?keyboardID={keyboardID}";
            var response = await GetAsync(keyboardUrl);

            var actual = ReadAsStringAsync(response).Length;
            Assert.True(actual > 1_048_576, $"Размер файла меньше 1 мегабайта: {actual}");
        }

        [Fact]
        public async void GetKeyboardFile_400_id()
        {
            var keyboardID = Guid.Parse("d296c943-4894-484a-b0c3-9b3783accba2");

            var httpClient = new HttpClient();
            var keyboardUrl = $"http://127.0.0.1:6005/api/v1/keyboards?keyboardID={keyboardID}";

            var response = await httpClient.GetAsync(keyboardUrl);
            var excepted = HttpStatusCode.BadRequest;
            Assert.Equal(excepted, response.StatusCode);
        }



        [Fact]
        public async void GetUserKeyboardsList_200()
        {
            var httpClient = new HttpClient();

            var signInUrl = "http://127.0.0.1:6005/api/v1/users/signIn";
            var email = "admin@keyboard.ru";
            var password = "11111111";

            var signInResponse = await httpClient.PostAsJsonAsync(signInUrl, new { email, password });
            var token = signInResponse.Content.ReadFromJsonAsync<AccessToken>().Result.accessToken;

            var keyboardUrl = "http://127.0.0.1:6005/api/v1/keyboards/auth";

            using (var request = new HttpRequestMessage(HttpMethod.Get, keyboardUrl))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var keyboardResponse = await httpClient.SendAsync(request);

                var actual = keyboardResponse.Content.ReadFromJsonAsync<List<SelectKeyboardDTO>>().Result.Count;
                Assert.NotEqual(0, actual);
            }
        }
    }
}