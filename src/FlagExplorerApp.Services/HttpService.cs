using System.Net.Http.Json;

namespace FlagExplorerApp.FlagExplorerApp.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            return await _httpClient.GetFromJsonAsync<T>(url);
        }
    }
}