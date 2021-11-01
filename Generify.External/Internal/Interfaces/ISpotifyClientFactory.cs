using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace Generify.External.Internal.Interfaces
{
    public interface ISpotifyClientFactory
    {
        Task<ISpotifyClient> CreateClientAsync();
    }
}
