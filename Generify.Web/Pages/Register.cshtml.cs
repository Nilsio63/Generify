using Generify.Models.Management;
using Generify.Services.Interfaces.Management;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Generify.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IUserService _userService;

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string PasswordRepeat { get; set; }

        public string ErrorMessage { get; private set; }

        public RegisterModel(ILogger<RegisterModel> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult OnGet(string errorMessage)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("./Index");
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
                return RedirectToPage("./Index");
            }

            TempData["UserName"] = UserName;

            if (!string.Equals(Password, PasswordRepeat, StringComparison.Ordinal))
            {
                return RedirectToPage(new { errorMessage = "Password didn't match password repetition" });
            }

            UserCreationResult result = await _userService.TryCreateUser(UserName, Password);

            if (result.IsSuccess)
            {
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, result.CreatedUser.Id)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                await Request.HttpContext.SignInAsync(new ClaimsPrincipal(identity));

                return RedirectToPage("./Index");
            }
            else
            {
                return RedirectToPage(new { errorMessage = result.ErrorMessage });
            }
        }
    }
}
