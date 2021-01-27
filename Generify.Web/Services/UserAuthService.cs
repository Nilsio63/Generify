using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Generify.Web.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IOptions<AuthenticationOptions> _authOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public UserAuthService(IOptions<AuthenticationOptions> authOptions,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _authOptions = authOptions;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            ClaimsPrincipal httpUser = _httpContextAccessor.HttpContext.User;

            if (!httpUser.Identity.IsAuthenticated)
            {
                return null;
            }

            string userId = httpUser.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value;

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
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            };

            var identity = new ClaimsIdentity(claims, _authOptions.Value.DefaultAuthenticateScheme);

            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(principal);
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}
