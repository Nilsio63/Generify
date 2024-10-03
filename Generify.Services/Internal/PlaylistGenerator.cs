using Generify.Models.Playlists;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;
using System.Threading.Tasks;

namespace Generify.Services.Internal;

public class PlaylistGenerator : IPlaylistGenerator
{
    private readonly IPlaylistGenerationContextBuilder _generationContextBuilder;
    private readonly IPlaylistSyncWorker _syncWorker;

    public PlaylistGenerator(IPlaylistGenerationContextBuilder generationContextBuilder,
        IPlaylistSyncWorker syncWorker)
    {
        _generationContextBuilder = generationContextBuilder;
        _syncWorker = syncWorker;
    }

    public async Task ExecuteGenerationAsync(PlaylistDefinition playlistDefinition)
    {
        PlaylistGenerationContext context = await _generationContextBuilder.CreateContextAsync(playlistDefinition);

        await _syncWorker.SyncTracksAsync(context);

        await _syncWorker.SyncSortingAsync(context);
    }
}
