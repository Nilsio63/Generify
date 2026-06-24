namespace Generify.External.Abstractions.Services;

public interface ILoginService
{
    string GetExternalLoginUrl();
    Task<string> GetRefreshTokenAsync(string accessToken);
}
