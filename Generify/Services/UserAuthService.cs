using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Generify.Services;

public interface IUserAuthService : IUserContextAccessor
{
    Task<bool> IsUserLoggedInAsync();
    Task LoginAsync(User user);
    Task LogoutAsync();
}

public class UserAuthService : IUserAuthService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public UserAuthService(
        AuthenticationStateProvider authStateProvider,
        IHttpContextAccessor httpContextAccessor,
        IUserService userService)
    {
        _authStateProvider = authStateProvider;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public async Task<bool> IsUserLoggedInAsync()
    {
        AuthenticationState authState = await _authStateProvider.GetAuthenticationStateAsync();

        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        AuthenticationState authState = await _authStateProvider.GetAuthenticationStateAsync();

        string? userId = authState.User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        User? user = await _userService.GetByIdAsync(userId);

        return user;
    }

    public async Task LoginAsync(User user)
    {
        if (_httpContextAccessor.HttpContext is not null)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(GetClaimsIdentity(user)));
        }
    }

    public async Task LogoutAsync()
    {
        if (_httpContextAccessor.HttpContext is not null)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }
    }

    private static ClaimsIdentity GetClaimsIdentity(User user)
    {
        return new ClaimsIdentity(
        [
            new Claim(ClaimTypes.Name, user.Id.ToString())
        ], CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
