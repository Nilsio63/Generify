using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;
using MoreLinq;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Internal
{
    public class PlaylistSyncWorker : IPlaylistSyncWorker
    {
        public async Task SyncTracksAsync(PlaylistGenerationContext context)
        {
            FullTrack[] toDelete = context.TargetTracks
                .Where(t => !context.SourceTracks.Any(s => s.Id == t.Id))
                .ToArray();

            FullTrack[] toAdd = context.SourceTracks
                .Where(s => !context.TargetTracks.Any(t => t.Id == s.Id))
                .ToArray();

            foreach (IEnumerable<FullTrack> batch in toDelete.Batch(100))
            {
                PlaylistRemoveItemsRequest req = new PlaylistRemoveItemsRequest
                {
                    Tracks = batch.Select(o => new PlaylistRemoveItemsRequest.Item { Uri = o.Uri }).ToList()
                };

                SnapshotResponse res = await context.Client.Playlists.RemoveItems(context.TargetPlaylist.Id, req);
            }

            foreach (IEnumerable<FullTrack> batch in toAdd.Batch(100))
            {
                SnapshotResponse res = await context.Client.Playlists.AddItems(context.TargetPlaylist.Id, new PlaylistAddItemsRequest(batch.Select(o => o.Uri).ToList()));
            }

            context.TargetTracks = context.TargetTracks
                .Except(toDelete)
                .Concat(toAdd)
                .ToList();
        }

        public async Task SyncSortingAsync(PlaylistGenerationContext context)
        {
            for (int i = 0; i < context.SourceTracks.Count; i++)
            {
                FullTrack track = context.SourceTracks[i];

                int curIndex = context.TargetTracks.FindIndex(o => o.Id == track.Id);

                if (curIndex != i)
                {
                    await context.Client.Playlists.ReorderItems(context.TargetPlaylist.Id, new PlaylistReorderItemsRequest(curIndex, i));

                    context.TargetTracks = context.TargetTracks.Move(curIndex, 1, i).ToList();
                }
            }
        }
    }
}
