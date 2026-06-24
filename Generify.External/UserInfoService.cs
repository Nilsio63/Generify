using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using SpotifyAPI.Web;

namespace Generify.External;

public class UserInfoService(ISpotifyClientFactory spotifyClientFactory) : IUserInfoService
{
    public async Task<string> GetSpotifyUserIdAsync(string refreshToken)
    {
        ISpotifyClient client = await spotifyClientFactory.CreateClientAsync(refreshToken);

        PrivateUser user = await client.UserProfile.Current();

        return user.Id;
    }
}
