using db.v1.main.DTOs;

using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class KeyboardControllerTests
    {
        [Fact]
        public void GetDefaultKeyboardsList()
        {
            var httpClient = new HttpClient();
            var keyboardUrl = "http://127.0.0.1:6005/api/v1/keyboards/default";

            var actual = httpClient.GetAsync(keyboardUrl).Result.Content.ReadFromJsonAsync<List<KeyboardDTO>>().Result.Count;
            var low = 1;
            var high = 999;
            Assert.InRange(actual, low, high);
        }
    }
}