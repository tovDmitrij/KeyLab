using db.v1.main.DTOs;

using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class KeyboardControllerTests
    {
        [Fact]
        public async void GetDefaultKeyboardsList_InRange()
        {
            var httpClient = new HttpClient();
            var keyboardUrl = "http://127.0.0.1:6005/api/v1/keyboards/default";

            var response = await httpClient.GetAsync(keyboardUrl);

            var actual = response.Content.ReadFromJsonAsync<List<KeyboardDTO>>().Result.Count;
            var low = 1;
            var high = 99999;
            Assert.InRange(actual, low, high);
        }
    }
}