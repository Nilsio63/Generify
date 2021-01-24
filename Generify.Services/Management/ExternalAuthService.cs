using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Services.Management
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly IExternalAuthSettings _externalAuthSettings;
        private readonly IUserRepository _userRepo;

        public ExternalAuthService(IExternalAuthSettings externalAuthSettings,
            IUserRepository userRepo)
        {
            _externalAuthSettings = externalAuthSettings;
            _userRepo = userRepo;
        }

        public string GetExternalLoginUrl(string userId)
        {
            var r = new LoginRequest(new Uri(_externalAuthSettings.CallbackUrl),
                _externalAuthSettings.ClientId,
                LoginRequest.ResponseType.Code)
            {
                State = userId,
                Scope = new[] { Scopes.UserTopRead, Scopes.UserReadEmail, Scopes.UserLibraryRead, Scopes.UserFollowRead, Scopes.PlaylistModifyPrivate, Scopes.PlaylistModifyPublic }
            };

            return r.ToUri().ToString();
        }

        public async Task SaveAccessTokenAsync(string userId, string accessToken)
        {
            User user = await _userRepo.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"Could not find user with id '{userId}'!");

            AuthorizationCodeTokenResponse response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(_externalAuthSettings.ClientId, _externalAuthSettings.ClientSecret, accessToken, new Uri(_externalAuthSettings.CallbackUrl)));

            user.RefreshToken = response.RefreshToken;

            await _userRepo.SaveAsync(user);
        }
    }
}
