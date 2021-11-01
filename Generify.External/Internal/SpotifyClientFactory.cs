using Generify.External.Abstractions.Settings;
using Generify.External.Internal.Interfaces;
using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace Generify.External.Internal
{
    public class SpotifyClientFactory : ISpotifyClientFactory
    {
        private readonly IExternalAuthSettings _externalAuthSettings;
        private readonly IUserContextAccessor _userContextAccessor;

        public SpotifyClientFactory(IExternalAuthSettings externalAuthSettings,
            IUserContextAccessor userContextAccessor)
        {
            _externalAuthSettings = externalAuthSettings;
            _userContextAccessor = userContextAccessor;
        }

        public async Task<ISpotifyClient> CreateClientAsync()
        {
            User user = await _userContextAccessor.GetCurrentUserAsync();

            if (user is null)
            {
                throw new InvalidOperationException("User is not logged in!");
            }

            return await CreateClientAsync(user.RefreshToken);
        }

        private async Task<ISpotifyClient> CreateClientAsync(string refreshToken)
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
