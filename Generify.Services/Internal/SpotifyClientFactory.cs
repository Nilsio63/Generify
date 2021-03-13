using Generify.Services.Abstractions.Management;
using Generify.Services.Internal.Interfaces;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace Generify.Services.Internal
{
    public class SpotifyClientFactory : ISpotifyClientFactory
    {
        private readonly IExternalAuthSettings _externalAuthSettings;

        public SpotifyClientFactory(IExternalAuthSettings externalAuthSettings)
        {
            _externalAuthSettings = externalAuthSettings;
        }

        public async Task<ISpotifyClient> CreateClientAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException($"'{nameof(refreshToken)}' cannot be null or whitespace", nameof(refreshToken));
            }

            AuthorizationCodeRefreshResponse codeRefreshResponse = await new OAuthClient().RequestToken(new AuthorizationCodeRefreshRequest(_externalAuthSettings.ClientId, _externalAuthSettings.ClientSecret, refreshToken));

            SpotifyClientConfig clientConfig = SpotifyClientConfig
                .CreateDefault()
                .WithToken(codeRefreshResponse.AccessToken);

            return new SpotifyClient(clientConfig);
        }
    }
}
