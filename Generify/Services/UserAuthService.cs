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

public class UserAuthService(
    AuthenticationStateProvider authStateProvider,
    IHttpContextAccessor httpContextAccessor,
    IUserService userService)
    : IUserAuthService
{
    public async Task<bool> IsUserLoggedInAsync()
    {
        AuthenticationState authState = await authStateProvider.GetAuthenticationStateAsync();

        return authState.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        AuthenticationState authState = await authStateProvider.GetAuthenticationStateAsync();

        string? userId = authState.User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        User? user = await userService.GetByIdAsync(userId);

        return user;
    }

    public async Task LoginAsync(User user)
    {
        if (httpContextAccessor.HttpContext is not null)
        {
            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(GetClaimsIdentity(user)));
        }
    }

    public async Task LogoutAsync()
    {
        if (httpContextAccessor.HttpContext is not null)
        {
            await httpContextAccessor.HttpContext.SignOutAsync();
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
