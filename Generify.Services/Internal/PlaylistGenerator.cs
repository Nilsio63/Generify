using Generify.Models.Playlists;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;

namespace Generify.Services.Internal;

public class PlaylistGenerator(
    IPlaylistGenerationContextBuilder generationContextBuilder,
    IPlaylistSyncWorker syncWorker)
    : IPlaylistGenerator
{
    public async Task ExecuteGenerationAsync(PlaylistDefinition playlistDefinition)
    {
        PlaylistGenerationContext context = await generationContextBuilder.CreateContextAsync(playlistDefinition);

        await syncWorker.SyncTracksAsync(context);

        await syncWorker.SyncSortingAsync(context);
    }
}
