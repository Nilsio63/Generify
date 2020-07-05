using Generify.Services.Abstractions.Management;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Generify.Web.Pages.Authentication
{
    public class AuthCallbackModel : PageModel
    {
        private readonly IExternalAuthService _externalAuthService;

        public AuthCallbackModel(IExternalAuthService externalAuthService)
        {
            _externalAuthService = externalAuthService;
        }

        public async Task OnGetAsync(string state, string code)
        {
            await _externalAuthService.SaveAccessTokenAsync(state, code);
        }
    }
}
