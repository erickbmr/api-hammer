namespace Lotest.Clients.Interfaces
{
    public interface IClient
    {
        Task<HttpResponseMessage> PostWithBasicAuthAsync(string url, string content, string username, string password);
        Task<HttpResponseMessage> PostWithTokenAsync(string url, string content, string token);
        Task<HttpResponseMessage> PostWithoutAuthAsync(string url, string content);
    }
}
