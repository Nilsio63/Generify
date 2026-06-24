using Generify.External.Abstractions.Services;
using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;

namespace Generify.Services.Playlists;

public class PlaylistOverviewService(
    IPlaylistInfoService playlistInfoService,
    IPlaylistDefinitionRepository playlistDefRepo)
    : IPlaylistOverviewService
{
    public async Task<List<PlaylistOverview>> GetAllByUserIdAsync(string userId)
    {
        List<PlaylistDefinition> definitions = await playlistDefRepo.GetAllByUserIdAsync(userId);

        return await definitions
            .ToAsyncEnumerable()
            .Select(async (o, _, _) => new
            {
                Def = o,
                Playlist = await playlistInfoService.GetPlaylistInfoAsync(o.TargetPlaylistId)
                    ?? throw new KeyNotFoundException($"Could not find playlist with id {o.TargetPlaylistId}")
            })
            .Select(o => new PlaylistOverview
            {
                Definition = o.Def,
                Name = o.Playlist.Name,
                Description = o.Playlist.Description,
                ImageUrl = o.Playlist.ImageUrl
            })
            .ToListAsync();
    }
}
