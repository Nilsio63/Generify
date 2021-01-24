using Generify.Models.Management;
using Generify.Web.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Generify.Web.Filters
{
    public class ExternalAuthFilter : IAsyncPageFilter
    {
        private readonly IUserAuthService _userAuthService;

        public ExternalAuthFilter(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            await next.Invoke();
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            if (context.HttpContext.Request.Path.Value.Contains("AuthNeeded") || context.HttpContext.Request.Path.Value.Contains("AuthCallback"))
            {
                return;
            }

            User user = await _userAuthService.GetCurrentUserAsync();

            if (user != null && string.IsNullOrWhiteSpace(user.RefreshToken))
            {
                context.HttpContext.Response.Redirect("/AuthNeeded");
            }
        }
    }
}
