using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        public IActionResult Get()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(dir)
                .Select(o => new
                {
                    name = o,
                    content = System.IO.File.ReadAllText(o)
                })
                .ToArray();

            return Ok(new Dictionary<string, object>
            {
                ["directory"] = dir,
                ["files"] = files
            });
        }
    }
}
