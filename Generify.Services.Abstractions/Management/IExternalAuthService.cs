using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Management;

public interface IExternalAuthService
{
    string GetExternalLoginUrl();
    Task<User> SaveAccessTokenAsync(string accessToken);
}
