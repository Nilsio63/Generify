using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;
using MoreLinq;

namespace Generify.Services.Internal;

public class PlaylistSyncWorker(IPlaylistEditService playlistEditService) : IPlaylistSyncWorker
{
    public async Task SyncTracksAsync(PlaylistGenerationContext context)
    {
        TrackInfo[] toDelete = context.TargetTracks
            .Where(t => !context.SourceTracks.Any(s => s.Id == t.Id))
            .ToArray();

        TrackInfo[] toAdd = context.SourceTracks
            .Where(s => !context.TargetTracks.Any(t => t.Id == s.Id))
            .ToArray();

        await playlistEditService.RemoveTracksFromPlaylistAsync(context.TargetPlaylist.Id, toDelete);
        await playlistEditService.AddTracksToPlaylistAsync(context.TargetPlaylist.Id, toAdd);

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
                await playlistEditService.ReorderTracksInPlaylistAsync(context.TargetPlaylist.Id, curIndex, i);

                context.TargetTracks = context.TargetTracks.Move(curIndex, 1, i).ToList();
            }
        }
    }
}
