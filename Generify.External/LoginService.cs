using Generify.External.Abstractions.Services;
using Generify.External.Abstractions.Settings;
using SpotifyAPI.Web;

namespace Generify.External;

public class LoginService(IExternalAuthSettings externalAuthSettings) : ILoginService
{
    public string GetExternalLoginUrl()
    {
        var request = new LoginRequest(new Uri(externalAuthSettings.CallbackUrl),
            externalAuthSettings.ClientId,
            LoginRequest.ResponseType.Code)
        {
            Scope = new[] { Scopes.UserTopRead, Scopes.UserReadEmail, Scopes.UserLibraryRead, Scopes.UserFollowRead, Scopes.PlaylistModifyPrivate, Scopes.PlaylistModifyPublic }
        };

        return request.ToUri().ToString();
    }

    public async Task<string> GetRefreshTokenAsync(string accessToken)
    {
        AuthorizationCodeTokenResponse response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(externalAuthSettings.ClientId, externalAuthSettings.ClientSecret, accessToken, new Uri(externalAuthSettings.CallbackUrl)));

        return response.RefreshToken;
    }
}
