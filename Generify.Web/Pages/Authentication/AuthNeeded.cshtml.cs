using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generify.Services.Abstractions.Management;
using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Generify.Web.Pages.Authentication
{
    public class AuthNeededModel : PageModel
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IExternalAuthService _externalAuthService;

        public string ExternalAuthLink { get; private set; }

        public AuthNeededModel(IUserAuthService userAuthService,
            IExternalAuthService externalAuthService)
        {
            _userAuthService = userAuthService;
            _externalAuthService = externalAuthService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userAuthService.GetCurrentUserAsync();

            if (user == null)
            {
                return RedirectToPage("/Index");
            }

            ExternalAuthLink = _externalAuthService.GetExternalLoginUrl(user.Id);

            return Page();
        }
    }
}
