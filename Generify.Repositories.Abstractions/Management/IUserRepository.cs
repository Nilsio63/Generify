using Generify.Models.Management;

namespace Generify.Repositories.Abstractions.Management;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetBySpotifyIdAsync(string spotifyUserId);
}
