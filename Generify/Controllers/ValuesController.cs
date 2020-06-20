using Generify.Models.Management;
using Generify.Repositories.Interfaces.Management;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Generify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public ValuesController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Get()
        {
            await _userRepository.SaveAsync(new User
            {
                AccessToken = "123"
            });

            return Ok();
        }
    }
}
