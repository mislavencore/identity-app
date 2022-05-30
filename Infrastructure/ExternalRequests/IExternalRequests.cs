using System.Threading.Tasks;

namespace Infrastructure.ExternalRequests
{
    public interface IExternalRequests
    {
        Task<string> GetRequestAsync(string url);
    }
}