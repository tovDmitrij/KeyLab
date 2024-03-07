using db.v1.main.DTOs.Switch;

using Microsoft.AspNetCore.WebUtilities;

using Xunit;

namespace misc.unit_tests.API
{
    public sealed class SwitchAPITests : APITest
    {
        [Theory]
        [InlineData(0)]
        public async void PositiveDefaultSwitchesTest(int excepted)
        {
            var switchUrl = $"{MainServiceAddress}/switches/models/default";
            var response = await GetAsync(switchUrl);
            var json = await ReadFromJsonAsync<List<SelectSwitchDTO>>(response);
            var actual = json.Count;

            Assert.True(actual != excepted, $"Список свитчей пустой: {actual}");
        }



        [Theory]
        [InlineData("b0ac9399-8eb1-4920-9366-82cbf7904eb1", 1_024)]
        public async void PositiveSwitchModelTest(Guid switchID, int excepted)
        {
            var switchUrl = $"{MainServiceAddress}/switches/models";
            var queryParams = new Dictionary<string, string>() { ["switchID"] = switchID.ToString() };
            var uri = QueryHelpers.AddQueryString(switchUrl, queryParams);

            var response = await GetAsync(uri);
            var file = await ReadAsStreamAsync(response);
            var actual = file.Length;

            Assert.True(actual > excepted, $"Размер файла меньше 10 килобайт: {actual}");
        }

        [Theory]
        [InlineData("b0ac9399-8eb1-4920-9366-82cbf7904eb2", 1_024)]
        public async void NegativeSwitchModelTest(Guid switchID, int excepted)
        {
            var switchUrl = $"{MainServiceAddress}/switches/models";
            var queryParams = new Dictionary<string, string>() { ["switchID"] = switchID.ToString() };
            var uri = QueryHelpers.AddQueryString(switchUrl, queryParams);

            var response = await GetAsync(uri);
            var file = await ReadAsStreamAsync(response);
            var actual = file.Length;

            Assert.True(actual < excepted, $"Несуществующий файл оказался существующим: {actual}");
        }



        [Theory]
        [InlineData("b0ac9399-8eb1-4920-9366-82cbf7904eb1", 1_024)]
        public async void PositiveSwitchSoundTest(Guid switchID, int excepted)
        {
            var soundUrl = $"{MainServiceAddress}/switches/sounds";
            var queryParams = new Dictionary<string, string>() { ["switchID"] = switchID.ToString() };
            var uri = QueryHelpers.AddQueryString(soundUrl, queryParams);

            var response = await GetAsync(uri);
            var file = await ReadAsStreamAsync(response);
            var actual = file.Length;

            Assert.True(actual > excepted, $"Размер файла меньше 10 килобайт: {actual}");
        }

        [Theory]
        [InlineData("b0ac9399-8eb1-4920-9366-82cbf7904eb2", 1_024)]
        public async void NegativeSwitchSoundTest(Guid switchID, int excepted)
        {
            var soundUrl = $"{MainServiceAddress}/switches/sounds";
            var queryParams = new Dictionary<string, string>() { ["switchID"] = switchID.ToString() };
            var uri = QueryHelpers.AddQueryString(soundUrl, queryParams);

            var response = await GetAsync(uri);
            var file = await ReadAsStreamAsync(response);
            var actual = file.Length;

            Assert.True(actual < excepted, $"Несуществующий файл оказался существующим: {actual}");
        }
    }
}