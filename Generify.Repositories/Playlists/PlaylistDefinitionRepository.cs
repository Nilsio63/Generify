using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Repositories.Playlists;

public class PlaylistDefinitionRepository : BaseRepository<PlaylistDefinition>, IPlaylistDefinitionRepository
{
    public PlaylistDefinitionRepository(GenerifyDataContext dataContext)
        : base(dataContext)
    {
    }

    public async Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId)
    {
        Guid userGuid = Guid.Parse(userId);

        return await BaseSelect
            .Where(o => o.UserId == userGuid)
            .ToListAsync();
    }

    public async Task<PlaylistDefinition?> GetByIdForUserAsync(string playlistId, string userId)
    {
        Guid playlistGuid = Guid.Parse(playlistId);
        Guid userGuid = Guid.Parse(userId);

        return await BaseSelect
            .Where(o => o.Id == playlistGuid
                && o.UserId == userGuid)
            .FirstOrDefaultAsync();
    }

    public async Task LoadDetailsAsync(PlaylistDefinition playlistDefinition)
    {
        EntityEntry<PlaylistDefinition> entry = DataContext.Entry(playlistDefinition);

        await entry.Reference(o => o.User).LoadAsync();
        await entry.Collection(o => o.PlaylistSources).LoadAsync();
        await entry.Collection(o => o.OrderInstructions).LoadAsync();

        playlistDefinition.PlaylistSources = playlistDefinition.PlaylistSources.OrderBy(o => o.OrderNr).ToList();
        playlistDefinition.OrderInstructions = playlistDefinition.OrderInstructions.OrderBy(o => o.OrderNr).ToList();
    }
}
