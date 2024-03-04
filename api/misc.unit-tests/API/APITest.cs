using System.Net.Http.Json;

namespace misc.unit_tests.API
{
    public abstract class APITest
    {
        protected static async Task<HttpResponseMessage?> GetAsync(string url)
        {
            var httpClient = GetHttpClient();
            var response = await httpClient.GetAsync(url);
            return response;
        }

        protected static async Task<HttpResponseMessage?> PostAsJsonAsync<T>(string url, T body)
        {
            var httpClient = GetHttpClient();
            var response = await httpClient.PostAsJsonAsync<T>(url, body);
            return response;
        }

        protected static T? ReadFromJsonAsync<T>(HttpResponseMessage response)
        {
            var data = response.Content.ReadFromJsonAsync<T>().Result;
            return data;
        }

        protected static string ReadAsStringAsync(HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return data;
        }



        private static HttpClient GetHttpClient() => new();
    }
}