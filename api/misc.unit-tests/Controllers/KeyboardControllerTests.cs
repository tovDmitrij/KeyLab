using db.v1.main.DTOs;

using misc.unit_tests.Responses;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class KeyboardControllerTests
    {
        [Fact]
        public async void GetDefaultKeyboardsList_200()
        {
            var httpClient = new HttpClient();
            var keyboardUrl = "http://127.0.0.1:6005/api/v1/keyboards/default";

            var response = await httpClient.GetAsync(keyboardUrl);

            var actual = response.Content.ReadFromJsonAsync<List<KeyboardInfoDTO>>().Result.Count;
            Assert.NotEqual(0, actual);
        }



        [Fact]
        public async void GetKeyboardFile_200()
        {
            var keyboardID = Guid.Parse("d296c943-4894-484a-b0c3-9b3783accbaa");

            var httpClient = new HttpClient();
            var keyboardUrl = $"http://127.0.0.1:6005/api/v1/keyboards?keyboardID={keyboardID}";

            var response = await httpClient.GetAsync(keyboardUrl);

            var actual = response.Content.ReadAsStringAsync().Result.Length;
            Assert.NotEqual(0, actual);
        }

        [Fact]
        public async void GetKeyboardFile_400()
        {
            var keyboardID = Guid.Parse("d296c943-4894-484a-b0c3-9b3783accba2");

            var httpClient = new HttpClient();
            var keyboardUrl = $"http://127.0.0.1:6005/api/v1/keyboards?keyboardID={keyboardID}";

            var response = await httpClient.GetAsync(keyboardUrl);
            var excepted = HttpStatusCode.BadRequest;
            Assert.Equal(excepted, response.StatusCode);
        }



        [Fact(Skip = "Not realized")]
        public async void AddKeyboard_200()
        {

        }

        [Fact(Skip = "Not realized")]
        public async void AddKeyboard_400()
        {

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

                var actual = keyboardResponse.Content.ReadFromJsonAsync<List<KeyboardInfoDTO>>().Result.Count;
                Assert.NotEqual(0, actual);
            }
        }
    }
}