using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserAuthService _userAuthService;

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; private set; }

        public LoginModel(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
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

            string error = await _userAuthService.TryLoginAsync(UserName, Password);

            if (string.IsNullOrWhiteSpace(error))
            {
                return RedirectToPage("./Index");
            }
            else
            {
                TempData["UserName"] = UserName;

                return RedirectToPage(new { errorMessage = error });
            }
        }
    }
}
