using Generify.Models.Management;
using Generify.Services;
using Generify.Services.Abstractions.Management;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Generify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("authCallback")]
    public async Task<IActionResult> AuthCallbackAsync(
        [FromServices] IUserAuthService userAuthService,
        [FromServices] IExternalAuthService externalAuthService,
        [FromQuery] string? code,
        [FromQuery] string? error)
    {
        if (!string.IsNullOrWhiteSpace(code))
        {
            User user = await externalAuthService.SaveAccessTokenAsync(code);

            await userAuthService.LoginAsync(user);
        }

        return Redirect("/authCallback?error=" + HttpUtility.UrlEncode(error));
    }

    [HttpGet("logout")]
    public async Task<IActionResult> LogoutAsync(
        [FromServices] IUserAuthService userAuthService)
    {
        await userAuthService.LogoutAsync();

        return Redirect("/logout");
    }
}
