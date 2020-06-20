using Generify.Models.Management;
using Generify.Repositories.Interfaces.Management;
using Generify.Services.Interfaces.Management;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace Generify.Services.Management
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepo;

        public AuthenticationService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public string GetExternalLoginUrl(string userId)
        {
            var r = new LoginRequest(new Uri("https://localhost:44383/api/authentication/callback"),
                "CLIENT_ID_HERE",
                LoginRequest.ResponseType.Code)
            {
                State = userId,
                Scope = new[] { Scopes.PlaylistModifyPublic }
            };

            return r.ToUri().ToString();
        }

        public async Task SaveAccessTokenAsync(string userId, string accessToken)
        {
            User user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                user = new User
                {
                    Id = userId,
                    AccessToken = accessToken
                };
            }

            await _userRepo.SaveAsync(user);
        }
    }
}
