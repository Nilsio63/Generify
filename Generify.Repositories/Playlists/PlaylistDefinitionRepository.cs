using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories.Playlists
{
    public class PlaylistDefinitionRepository : BaseRepository<PlaylistDefinition>, IPlaylistDefinitionRepository
    {
        public PlaylistDefinitionRepository(GenerifyDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId)
        {
            return await BaseSelect
                .Where(o => o.User.Id == userId)
                .ToListAsync();
        }
    }
}
