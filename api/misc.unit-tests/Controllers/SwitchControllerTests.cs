using db.v1.main.DTOs;

using Microsoft.AspNetCore.WebUtilities;

using System.Net;
using System.Net.Http.Json;

using Xunit;

namespace misc.unit_tests.Controllers
{
    public sealed class SwitchControllerTests
    {
        [Fact]
        public async void GetSwitchList_200()
        {
            var httpClient = new HttpClient();
            var switchUrl = "http://127.0.0.1:6005/api/v1/switches/models/default";

            var response = await httpClient.GetFromJsonAsync<List<SwitchInfoDTO>>(switchUrl);
            Assert.NotNull(response);
        }



        [Fact]
        public async void GetSwitchModelFile_200()
        {
            var httpClient = new HttpClient();
            var switchID = "b0ac9399-8eb1-4920-9366-82cbf7904eb1";
            var switchUrl = "http://127.0.0.1:6005/api/v1/switches/models";

            var queryParams = new Dictionary<string, string>()
            {
                ["switchID"] = switchID
            };
            var uri = QueryHelpers.AddQueryString(switchUrl, queryParams);

            var response = await httpClient.GetAsync(uri);

            var file = await response.Content.ReadAsStreamAsync();
            Assert.InRange(file.Length, 10_000, 999_999);
        }

        [Fact]
        public async void GetSwitchModelFile_400_id()
        {
            var httpClient = new HttpClient();
            var switchID = "b0ac9399-8eb1-4920-9366-82cbf7904eb2";
            var switchUrl = "http://127.0.0.1:6005/api/v1/switches/models";

            var queryParams = new Dictionary<string, string>()
            {
                ["switchID"] = switchID
            };
            var uri = QueryHelpers.AddQueryString(switchUrl, queryParams);

            var response = await httpClient.GetAsync(uri);

            var actual = response.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.Equal(expected, actual);
        }



        [Fact]
        public async void GetSwitchSoundFile_200()
        {
            var httpClient = new HttpClient();
            var switchID = "b0ac9399-8eb1-4920-9366-82cbf7904eb1";
            var soundUrl = "http://127.0.0.1:6005/api/v1/switches/sounds";

            var queryParams = new Dictionary<string, string>()
            {
                ["switchID"] = switchID
            };
            var uri = QueryHelpers.AddQueryString(soundUrl, queryParams);

            var response = await httpClient.GetAsync(uri);

            var file = await response.Content.ReadAsStreamAsync();
            Assert.InRange(file.Length, 1_000, 999_999);
        }

        [Fact]
        public async void GetSwitchSoundFile_400_id()
        {
            var httpClient = new HttpClient();
            var switchID = "b0ac9399-8eb1-4920-9366-82cbf7904eb2";
            var soundUrl = "http://127.0.0.1:6005/api/v1/switches/sounds";

            var queryParams = new Dictionary<string, string>()
            {
                ["switchID"] = switchID
            };
            var uri = QueryHelpers.AddQueryString(soundUrl, queryParams);

            var response = await httpClient.GetAsync(uri);

            var actual = response.StatusCode;
            var expected = HttpStatusCode.BadRequest;
            Assert.Equal(expected, actual);
        }
    }
}