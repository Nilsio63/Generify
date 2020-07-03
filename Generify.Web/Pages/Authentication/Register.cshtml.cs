using Generify.Models.Management;
using Generify.Services.Abstractions.Management;
using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Generify.Web.Pages.Authentication
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IUserAuthService _userAuthService;
        private readonly IUserService _userService;

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string PasswordRepeat { get; set; }

        public string ErrorMessage { get; private set; }

        public RegisterModel(ILogger<RegisterModel> logger,
            IUserAuthService userAuthService,
            IUserService userService)
        {
            _logger = logger;
            _userAuthService = userAuthService;
            _userService = userService;
        }

        public IActionResult OnGet(string errorMessage)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            ErrorMessage = errorMessage;

            UserName = TempData.TryGetValue("UserName", out object userName) ? userName?.ToString() : null;

            TempData.Remove("UserName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            if (!string.Equals(Password, PasswordRepeat, StringComparison.Ordinal))
            {
                return RedirectToPage(new { errorMessage = "Password didn't match password repetition" });
            }

            UserCreationResult result = await _userService.TryCreateUser(UserName, Password);

            if (result.IsSuccess)
            {
                await _userAuthService.LoginAsync(result.CreatedUser);

                return RedirectToPage("/Index");
            }
            else
            {
                TempData["UserName"] = UserName;

                return RedirectToPage(new { errorMessage = result.ErrorMessage });
            }
        }
    }
}