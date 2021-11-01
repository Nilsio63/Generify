using System.Threading.Tasks;

namespace Generify.External.Abstractions.Services
{
    public interface ILoginService
    {
        string GetExternalLoginUrl(string userId);
        Task<string> GetRefreshTokenAsync(string accessToken);
    }
}
