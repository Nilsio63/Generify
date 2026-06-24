using Generify.External.Abstractions.Settings;
using Generify.External.Internal.Interfaces;
using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using SpotifyAPI.Web;

namespace Generify.External.Internal;

public class SpotifyClientFactory(
    IExternalAuthSettings externalAuthSettings,
    IUserContextAccessor userContextAccessor)
    : ISpotifyClientFactory
{
    public async Task<ISpotifyClient> CreateClientAsync()
    {
        User? user = await userContextAccessor.GetCurrentUserAsync();

        if (user is null)
        {
            throw new InvalidOperationException("User is not logged in!");
        }

        return await CreateClientAsync(user.RefreshToken);
    }

    public async Task<ISpotifyClient> CreateClientAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException($"'{nameof(refreshToken)}' cannot be null or whitespace", nameof(refreshToken));
        }

        AuthorizationCodeRefreshResponse codeRefreshResponse = await new OAuthClient().RequestToken(new AuthorizationCodeRefreshRequest(externalAuthSettings.ClientId, externalAuthSettings.ClientSecret, refreshToken));

        SpotifyClientConfig clientConfig = SpotifyClientConfig
            .CreateDefault()
            .WithToken(codeRefreshResponse.AccessToken);

        return new SpotifyClient(clientConfig);
    }
}
