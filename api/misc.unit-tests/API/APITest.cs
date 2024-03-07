using System.Net.Http.Json;

namespace misc.unit_tests.API
{
    public abstract class APITest
    {
        protected static string MainServiceAddress { get => "http://127.0.0.1:6001/api/v1"; }
        protected static string EmailServiceAddress { get => "http://127.0.0.1:6002/api/v1"; }



        protected static async Task<HttpResponseMessage?> GetAsync(string url) => await GetHttpClient().GetAsync(url);
        protected static async Task<HttpResponseMessage?> GetAsync(HttpRequestMessage msg) => await GetHttpClient().SendAsync(msg);

        protected static async Task<HttpResponseMessage?> PostAsJsonAsync<T>(string url, T body) => await GetHttpClient().PostAsJsonAsync(url, body);

        protected static async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response) => await response.Content.ReadFromJsonAsync<T>();
        protected static async Task<string> ReadAsStringAsync(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
        protected static async Task<Stream?> ReadAsStreamAsync(HttpResponseMessage response) => await response.Content.ReadAsStreamAsync();



        private static HttpClient GetHttpClient() => new();
    }
}