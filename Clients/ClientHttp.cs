using Lotest.Clients.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace Lotest.Clients
{
    public class ClientHttp : IClient
    {
        private readonly HttpClient _httpClient;

        public ClientHttp (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostWithBasicAuthAsync(string url, string content, string username, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> PostWithoutAuthAsync(string url, string content)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> PostWithTokenAsync(string url, string content, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, httpContent);
        }
    }
}
