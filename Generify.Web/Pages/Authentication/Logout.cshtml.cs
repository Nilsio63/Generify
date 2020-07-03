using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        private readonly IUserAuthService _userAuthService;

        public LogoutModel(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Page();
            }

            await _userAuthService.LogoutAsync();

            return RedirectToPage();
        }
    }
}
