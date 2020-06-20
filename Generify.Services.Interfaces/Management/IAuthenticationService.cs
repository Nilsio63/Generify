using System.Threading.Tasks;

namespace Generify.Services.Interfaces.Management
{
    public interface IAuthenticationService
    {
        string GetExternalLoginUrl(string userId);
        Task SaveAccessTokenAsync(string userId, string accessToken);
    }
}
