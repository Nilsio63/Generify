using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Microsoft.EntityFrameworkCore;

namespace Generify.Repositories.Management;

public class UserRepository(GenerifyDataContext dataContext) : BaseRepository<User>(dataContext), IUserRepository
{
    public async Task<User?> GetBySpotifyIdAsync(string spotifyUserId)
    {
        return await BaseSelect
            .Where(o => o.SpotifyId == spotifyUserId)
            .FirstOrDefaultAsync();
    }
}
