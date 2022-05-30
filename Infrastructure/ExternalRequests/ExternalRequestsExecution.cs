using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.ExternalRequests
{
    public class ExternalRequestsExecution : IExternalRequests
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalRequestsExecution(IHttpClientFactory httpClientFactory) 
            => _httpClientFactory = httpClientFactory;

        public async Task<string> GetRequestAsync(string url)
        {
            var result = await _httpClientFactory.CreateClient().GetAsync(url);
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }
    }
}