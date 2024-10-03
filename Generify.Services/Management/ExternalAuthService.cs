using Generify.External.Abstractions.Services;
using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Generify.Services.Abstractions.Management;
using System.Threading.Tasks;

namespace Generify.Services.Management;

public class ExternalAuthService : IExternalAuthService
{
    private readonly ILoginService _loginService;
    private readonly IUserInfoService _userInfoService;
    private readonly IUserRepository _userRepo;

    public ExternalAuthService(
        ILoginService loginService,
        IUserInfoService userInfoService,
        IUserRepository userRepo)
    {
        _loginService = loginService;
        _userInfoService = userInfoService;
        _userRepo = userRepo;
    }

    public string GetExternalLoginUrl()
    {
        return _loginService.GetExternalLoginUrl();
    }

    public async Task<User> SaveAccessTokenAsync(string accessToken)
    {
        string refreshToken = await _loginService.GetRefreshTokenAsync(accessToken);

        string spotifyUserId = await _userInfoService.GetSpotifyUserIdAsync(refreshToken);

        User user = await _userRepo.GetBySpotifyIdAsync(spotifyUserId)
            ?? new()
            {
                SpotifyId = spotifyUserId
            };

        user.RefreshToken = refreshToken;

        await _userRepo.SaveAsync(user);

        return user;
    }
}
