using Generify.External.Abstractions.Services;
using Generify.External.Abstractions.Settings;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace Generify.External
{
    public class LoginService : ILoginService
    {
        private readonly IExternalAuthSettings _externalAuthSettings;

        public LoginService(IExternalAuthSettings externalAuthSettings)
        {
            _externalAuthSettings = externalAuthSettings;
        }

        public string GetExternalLoginUrl()
        {
            var request = new LoginRequest(new Uri(_externalAuthSettings.CallbackUrl),
                _externalAuthSettings.ClientId,
                LoginRequest.ResponseType.Code)
            {
                Scope = new[] { Scopes.UserTopRead, Scopes.UserReadEmail, Scopes.UserLibraryRead, Scopes.UserFollowRead, Scopes.PlaylistModifyPrivate, Scopes.PlaylistModifyPublic }
            };

            return request.ToUri().ToString();
        }

        public async Task<string> GetRefreshTokenAsync(string accessToken)
        {
            AuthorizationCodeTokenResponse response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(_externalAuthSettings.ClientId, _externalAuthSettings.ClientSecret, accessToken, new Uri(_externalAuthSettings.CallbackUrl)));

            return response.RefreshToken;
        }
    }
}
