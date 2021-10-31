using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace Generify.External.Abstractions
{
    public interface ISpotifyClientFactory
    {
        Task<ISpotifyClient> CreateClientAsync();
        Task<ISpotifyClient> CreateClientAsync(string refreshToken);
    }
}
