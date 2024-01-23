using component.v1.exceptions;

using db.v1.main.DTOs;

using System.Net;
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



        [Fact(Skip = "Not realized")]
        public async void GetUserKeyboardsList_200()
        {

        }

        [Fact(Skip = "Not realized")]
        public async void GetUserKeyboardsList_400()
        {

        }

    }
}