using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace Generify.Services.Internal.Interfaces
{
    public interface ISpotifyClientFactory
    {
        Task<ISpotifyClient> CreateClientAsync(string refreshToken);
    }
}
