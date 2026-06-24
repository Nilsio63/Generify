using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using MoreLinq;
using SpotifyAPI.Web;

namespace Generify.External;

public class PlaylistEditService(ISpotifyClientFactory spotifyClientFactory) : IPlaylistEditService
{
    public async Task AddTracksToPlaylistAsync(string playlistId, IEnumerable<TrackInfo> tracks)
    {
        ISpotifyClient client = await spotifyClientFactory.CreateClientAsync();

        foreach (IEnumerable<TrackInfo> batch in tracks.Batch(100))
        {
            await client.Playlists.AddPlaylistItems(playlistId, new PlaylistAddItemsRequest([.. batch.Select(o => o.Uri)]));
        }
    }

    public async Task RemoveTracksFromPlaylistAsync(string playlistId, IEnumerable<TrackInfo> tracks)
    {
        ISpotifyClient client = await spotifyClientFactory.CreateClientAsync();

        foreach (IEnumerable<TrackInfo> batch in tracks.Batch(100))
        {
            PlaylistRemoveItemsRequestV2 req = new PlaylistRemoveItemsRequestV2
            {
                Items = batch.Select(o => new PlaylistRemoveItemsRequestV2.Item { Uri = o.Uri }).ToList()
            };

            await client.Playlists.RemovePlaylistItems(playlistId, req);
        }
    }

    public async Task ReorderTracksInPlaylistAsync(string playlistId, int startIndex, int insertBeforeIndex)
    {
        ISpotifyClient client = await spotifyClientFactory.CreateClientAsync();

        await client.Playlists.UpdatePlaylistItems(playlistId, new PlaylistReorderItemsRequest(startIndex, insertBeforeIndex));
    }
}
