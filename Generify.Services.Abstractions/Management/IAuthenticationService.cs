using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Management
{
    public interface IAuthenticationService
    {
        string GetExternalLoginUrl(string userId);
        Task SaveAccessTokenAsync(string userId, string accessToken);
    }
}
