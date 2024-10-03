using Generify.Models.Management;
using Generify.Repositories.Abstractions.Management;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories.Management;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(GenerifyDataContext dataContext)
        : base(dataContext)
    {
    }

    public async Task<User?> GetBySpotifyIdAsync(string spotifyUserId)
    {
        return await BaseSelect
            .Where(o => o.SpotifyId == spotifyUserId)
            .FirstOrDefaultAsync();
    }
}
