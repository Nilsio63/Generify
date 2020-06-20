using Generify.Models.Management;
using Generify.Repositories.Interfaces.Management;
using Generify.Services.Interfaces.Management;
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
