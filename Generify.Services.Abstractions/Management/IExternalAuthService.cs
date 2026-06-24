using Generify.Models.Management;

namespace Generify.Services.Abstractions.Management;

public interface IExternalAuthService
{
    string GetExternalLoginUrl();
    Task<User> SaveAccessTokenAsync(string accessToken);
}
