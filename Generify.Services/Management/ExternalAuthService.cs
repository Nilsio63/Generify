using Generify.External.Abstractions.Services;
using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Services.Management
{
    public class ExternalAuthService : IExternalAuthService
    {
        private readonly ILoginService _loginService;
        private readonly IUserRepository _userRepo;

        public ExternalAuthService(ILoginService loginService,
            IUserRepository userRepo)
        {
            _loginService = loginService;
            _userRepo = userRepo;
        }

        public string GetExternalLoginUrl(string userId)
        {
            return _loginService.GetExternalLoginUrl(userId);
        }

        public async Task SaveAccessTokenAsync(string userId, string accessToken)
        {
            User user = await _userRepo.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"Could not find user with id '{userId}'!");

            user.RefreshToken = await _loginService.GetRefreshTokenAsync(accessToken);

            await _userRepo.SaveAsync(user);
        }
    }
}
