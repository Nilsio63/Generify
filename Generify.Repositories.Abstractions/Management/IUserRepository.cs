using Generify.Models.Management;
using System.Threading.Tasks;

namespace Generify.Repositories.Abstractions.Management
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetBySpotifyIdAsync(string spotifyUserId);
    }
}
