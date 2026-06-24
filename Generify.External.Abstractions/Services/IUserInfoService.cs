namespace Generify.External.Abstractions.Services;

public interface IUserInfoService
{
    Task<string> GetSpotifyUserIdAsync(string refreshToken);
}
