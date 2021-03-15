using Blazored.LocalStorage;
using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Generify.Blazor.Services
{
    public class GenerifyAuthenticationStateProvider : AuthenticationStateProvider, IGenerifyAuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IUserService _userService;

        public GenerifyAuthenticationStateProvider(ILocalStorageService localStorageService,
            IUserService userService)
        {
            _localStorageService = localStorageService;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string userId = await _localStorageService.GetItemAsync<string>("userId");

            User user = await _userService.GetByIdAsync(userId);

            var claimsIdentity = user != null
                ? GetClaimsIdentity(user)
                : new ClaimsIdentity();

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);
        }

        public async Task SetAuthenticatedAsync(User user)
        {
            await _localStorageService.SetItemAsync("userId", user.Id);

            ClaimsIdentity identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task RemoveAuthenticatedAsync()
        {
            await _localStorageService.RemoveItemAsync("userId");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            }, "apiauth_type");
        }
    }
}
