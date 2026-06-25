using Generify.Services.Internal.Models;

namespace Generify.Services.Internal.Interfaces;

public interface IPlaylistSyncWorker
{
    Task SyncTracksAsync(PlaylistGenerationContext context);
    Task SyncSortingAsync(PlaylistGenerationContext context);
}
