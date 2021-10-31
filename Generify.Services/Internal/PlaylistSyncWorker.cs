using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;
using MoreLinq;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Internal
{
    public class PlaylistSyncWorker : IPlaylistSyncWorker
    {
        private readonly IPlaylistEditService _playlistEditService;

        public PlaylistSyncWorker(IPlaylistEditService playlistEditService)
        {
            _playlistEditService = playlistEditService;
        }

        public async Task SyncTracksAsync(PlaylistGenerationContext context)
        {
            TrackInfo[] toDelete = context.TargetTracks
                .Where(t => !context.SourceTracks.Any(s => s.Id == t.Id))
                .ToArray();

            TrackInfo[] toAdd = context.SourceTracks
                .Where(s => !context.TargetTracks.Any(t => t.Id == s.Id))
                .ToArray();

            await _playlistEditService.RemoveTracksFromPlaylistAsync(context.TargetPlaylist.Id, toDelete);
            await _playlistEditService.AddTracksToPlaylistAsync(context.TargetPlaylist.Id, toAdd);

            context.TargetTracks = context.TargetTracks
                .Except(toDelete)
                .Concat(toAdd)
                .ToList();
        }

        public async Task SyncSortingAsync(PlaylistGenerationContext context)
        {
            for (int i = 0; i < context.SourceTracks.Count; i++)
            {
                TrackInfo track = context.SourceTracks[i];

                int curIndex = context.TargetTracks.FindIndex(o => o.Id == track.Id);

                if (curIndex != i)
                {
                    await _playlistEditService.ReorderTracksInPlaylistAsync(context.TargetPlaylist.Id, curIndex, i);

                    context.TargetTracks = context.TargetTracks.Move(curIndex, 1, i).ToList();
                }
            }
        }
    }
}
