using Generify.Services.Interfaces.Management;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Generify.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallbackAsync([FromQuery] string code, [FromQuery] string state)
        {
            await _authService.SaveAccessTokenAsync(state, code);

            return Redirect("https://www.google.de");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var url = _authService.GetExternalLoginUrl("abc123");

            return Redirect(url);
        }
    }
}
