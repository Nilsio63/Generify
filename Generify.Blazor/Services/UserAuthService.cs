using Blazored.LocalStorage;
using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using System.Threading.Tasks;

namespace Generify.Blazor.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IGenerifyAuthenticationStateProvider _authStateProvider;
        private readonly IUserService _userService;

        public UserAuthService(ILocalStorageService localStorageService,
            IGenerifyAuthenticationStateProvider authStateProvider,
            IUserService userService)
        {
            _localStorageService = localStorageService;
            _authStateProvider = authStateProvider;
            _userService = userService;
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            string userId = await _localStorageService.GetItemAsync<string>("userId");

            return !string.IsNullOrWhiteSpace(userId);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            string userId = await _localStorageService.GetItemAsync<string>("userId");

            if (string.IsNullOrWhiteSpace(userId))
            {
                return null;
            }

            User user = await _userService.GetByIdAsync(userId);

            return user;
        }

        public async Task<string> TryLoginAsync(string userName, string password)
        {
            User user = await _userService.GetByLoginAsync(userName, password);

            if (user == null)
            {
                return "Username or password is wrong";
            }

            await LoginAsync(user);

            return null;
        }

        public async Task LoginAsync(User user)
        {
            await _authStateProvider.SetAuthenticatedAsync(user);
        }

        public async Task LogoutAsync()
        {
            await _authStateProvider.RemoveAuthenticatedAsync();
        }
    }
}
