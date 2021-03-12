using Generify.Models.Management;
using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserAuthService _userAuthService;

        public User GenerifyUser { get; set; }

        public IndexModel(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task OnGet()
        {
            GenerifyUser = await _userAuthService.GetCurrentUserAsync();
        }
    }
}
