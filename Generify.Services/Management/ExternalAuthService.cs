using Generify.External.Abstractions.Services;
using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;

namespace Generify.Services.Management;

public class ExternalAuthService(
    ILoginService loginService,
    IUserInfoService userInfoService,
    IUserRepository userRepo)
    : IExternalAuthService
{
    public string GetExternalLoginUrl()
    {
        return loginService.GetExternalLoginUrl();
    }

    public async Task<User> SaveAccessTokenAsync(string accessToken)
    {
        string refreshToken = await loginService.GetRefreshTokenAsync(accessToken);

        string spotifyUserId = await userInfoService.GetSpotifyUserIdAsync(refreshToken);

        User user = await userRepo.GetBySpotifyIdAsync(spotifyUserId)
            ?? new()
            {
                SpotifyId = spotifyUserId
            };

        user.RefreshToken = refreshToken;

        await userRepo.SaveAsync(user);

        return user;
    }
}
