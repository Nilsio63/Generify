using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task LoadDetailsAsync(PlaylistDefinition playlistDefinition)
        {
            EntityEntry<PlaylistDefinition> entry = DataContext.Entry(playlistDefinition);

            await entry.Reference(o => o.User).LoadAsync();
            await entry.Collection(o => o.PlaylistSources).LoadAsync();
            await entry.Collection(o => o.OrderInstructions).LoadAsync();
        }
    }
}
