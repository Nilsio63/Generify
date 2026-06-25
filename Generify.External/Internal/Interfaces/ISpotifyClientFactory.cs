using SpotifyAPI.Web;

namespace Generify.External.Internal.Interfaces;

public interface ISpotifyClientFactory
{
    Task<ISpotifyClient> CreateClientAsync();
    Task<ISpotifyClient> CreateClientAsync(string refreshToken);
}
