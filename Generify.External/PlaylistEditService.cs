using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using MoreLinq;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.External
{
    public class PlaylistEditService : IPlaylistEditService
    {
        private readonly ISpotifyClientFactory _spotifyClientFactory;

        public PlaylistEditService(ISpotifyClientFactory spotifyClientFactory)
        {
            _spotifyClientFactory = spotifyClientFactory;
        }

        public async Task AddTracksToPlaylistAsync(string playlistId, IEnumerable<TrackInfo> tracks)
        {
            ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

            foreach (IEnumerable<TrackInfo> batch in tracks.Batch(100))
            {
                await client.Playlists.AddItems(playlistId, new PlaylistAddItemsRequest(batch.Select(o => o.Uri).ToList()));
            }
        }

        public async Task RemoveTracksFromPlaylistAsync(string playlistId, IEnumerable<TrackInfo> tracks)
        {
            ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

            foreach (IEnumerable<TrackInfo> batch in tracks.Batch(100))
            {
                PlaylistRemoveItemsRequest req = new PlaylistRemoveItemsRequest
                {
                    Tracks = batch.Select(o => new PlaylistRemoveItemsRequest.Item { Uri = o.Uri }).ToList()
                };

                await client.Playlists.RemoveItems(playlistId, req);
            }
        }

        public async Task ReorderTracksInPlaylistAsync(string playlistId, int startIndex, int insertBeforeIndex)
        {
            ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

            await client.Playlists.ReorderItems(playlistId, new PlaylistReorderItemsRequest(startIndex, insertBeforeIndex));
        }
    }
}
