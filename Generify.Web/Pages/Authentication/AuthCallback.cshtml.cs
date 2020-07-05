using Generify.Services.Abstractions.Management;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages.Authentication
{
    public class AuthCallbackModel : PageModel
    {
        private readonly IExternalAuthService _externalAuthService;

        public string ErrorMessage { get; private set; }

        public AuthCallbackModel(IExternalAuthService externalAuthService)
        {
            _externalAuthService = externalAuthService;
        }

        public async Task OnGetAsync(string state, string code, string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                ErrorMessage = error;
            }
            else
            {
                await _externalAuthService.SaveAccessTokenAsync(state, code);
            }
        }
    }
}
