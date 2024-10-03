using Generify.External.Abstractions.Services;
using Generify.Models.Playlists;
using Generify.Repositories.Abstractions.Playlists;
using Generify.Services.Abstractions.Playlists;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Playlists;

public class PlaylistOverviewService : IPlaylistOverviewService
{
    private readonly IPlaylistInfoService _playlistInfoService;
    private readonly IPlaylistDefinitionRepository _playlistDefRepo;

    public PlaylistOverviewService(IPlaylistInfoService playlistInfoService,
        IPlaylistDefinitionRepository playlistDefRepo)
    {
        _playlistInfoService = playlistInfoService;
        _playlistDefRepo = playlistDefRepo;
    }

    public async Task<List<PlaylistOverview>> GetAllByUserIdAsync(string userId)
    {
        List<PlaylistDefinition> definitions = await _playlistDefRepo.GetAllByUserIdAsync(userId);

        return await definitions
            .ToAsyncEnumerable()
            .SelectAwait(async o => new
            {
                Def = o,
                Playlist = await _playlistInfoService.GetPlaylistInfoAsync(o.TargetPlaylistId)
            })
            .Select(o => new PlaylistOverview
            {
                Definition = o.Def,
                Name = o.Playlist.Name,
                Description = o.Playlist.Description
            })
            .ToListAsync();
    }
}
