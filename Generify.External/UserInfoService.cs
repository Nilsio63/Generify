using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using SpotifyAPI.Web;
using System.Threading.Tasks;

namespace Generify.External;

public class UserInfoService : IUserInfoService
{
    private readonly ISpotifyClientFactory _spotifyClientFactory;

    public UserInfoService(ISpotifyClientFactory spotifyClientFactory)
    {
        _spotifyClientFactory = spotifyClientFactory;
    }

    public async Task<string> GetSpotifyUserIdAsync(string refreshToken)
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync(refreshToken);

        PrivateUser user = await client.UserProfile.Current();

        return user.Id;
    }
}
