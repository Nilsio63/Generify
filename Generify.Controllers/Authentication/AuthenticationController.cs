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
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
        {
            await _authService.SaveAccessTokenAsync(state, code);

            return Ok(state);
        }
    }
}
