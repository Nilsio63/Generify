using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages.Authentication
{
    public class AuthNeededModel : PageModel
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IExternalAuthService _externalAuthService;

        public AuthNeededModel(IUserAuthService userAuthService,
            IExternalAuthService externalAuthService)
        {
            _userAuthService = userAuthService;
            _externalAuthService = externalAuthService;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User user = await _userAuthService.GetCurrentUserAsync();

            if (user == null)
            {
                return RedirectToPage("/Index");
            }

            string externalAuthLink = _externalAuthService.GetExternalLoginUrl(user.Id);

            return Redirect(externalAuthLink);
        }
    }
}
