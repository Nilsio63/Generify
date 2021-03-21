using Generify.Services.Internal.Models;
using System.Threading.Tasks;

namespace Generify.Services.Internal.Interfaces
{
    public interface IPlaylistSyncWorker
    {
        Task SyncTracksAsync(PlaylistGenerationContext context);
        Task SyncSortingAsync(PlaylistGenerationContext context);
    }
}
