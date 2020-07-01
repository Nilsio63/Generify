using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages
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
                return RedirectToPage("./Index");
            }

            await _userAuthService.LogoutAsync();

            return Page();
        }
    }
}
