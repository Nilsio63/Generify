using Generify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Generify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("logout")]
    public async Task<IActionResult> LogoutAsync(
        [FromServices] IUserAuthService userAuthService)
    {
        await userAuthService.LogoutAsync();

        return Redirect("/logout");
    }
}
