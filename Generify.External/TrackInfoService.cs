using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.External.Internal.Interfaces;
using MoreLinq;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.External;

public class TrackInfoService : ITrackInfoService
{
    private readonly ISpotifyClientFactory _spotifyClientFactory;

    public TrackInfoService(ISpotifyClientFactory spotifyClientFactory)
    {
        _spotifyClientFactory = spotifyClientFactory;
    }

    public async Task<TrackInfo?> GetByIdAsync(string trackId)
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

        FullTrack track = await client.Tracks.Get(trackId);

        return Map(track);
    }

    public async Task<List<TrackInfo>> GetByAlbumIdAsync(string albumId)
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

        Paging<SimpleTrack> albumPaginate = await client.Albums.GetTracks(albumId);

        List<SimpleTrack> tracksFromAlbum = await client.Paginate(albumPaginate).ToListAsync();

        return await tracksFromAlbum
            .Batch(50)
            .ToAsyncEnumerable()
            .SelectAwait(async o => await client.Tracks.GetSeveral(new TracksRequest(o.Select(p => p.Id).ToList())))
            .SelectMany(o => o.Tracks.ToAsyncEnumerable())
            .Select(Map)
            .ToListAsync();
    }

    public async Task<List<TrackInfo>> GetByArtistIdAsync(string artistId)
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

        Paging<SimpleAlbum> albumPaginate = await client.Artists.GetAlbums(artistId);

        List<SimpleAlbum> simpleAlbumList = await client.Paginate(albumPaginate)
            .ToListAsync();

        List<FullAlbum> fullAlbumList = await simpleAlbumList
            .Batch(20)
            .ToAsyncEnumerable()
            .SelectAwait(async o => await client.Albums.GetSeveral(new AlbumsRequest(o.Select(p => p.Id).ToList())))
            .SelectMany(o => o.Albums.ToAsyncEnumerable())
            .ToListAsync();

        List<SimpleTrack> simpleTrackList = await fullAlbumList
            .ToAsyncEnumerable()
            .SelectMany(o => client.Paginate(o.Tracks))
            .ToListAsync();

        return await simpleTrackList
            .Batch(50)
            .ToAsyncEnumerable()
            .SelectAwait(async o => await client.Tracks.GetSeveral(new TracksRequest(o.Select(p => p.Id).ToList())))
            .SelectMany(o => o.Tracks.ToAsyncEnumerable())
            .Select(Map)
            .ToListAsync();
    }

    public async Task<List<TrackInfo>> GetByPlaylistIdAsync(string playlistId)
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

        Paging<PlaylistTrack<IPlayableItem>> playlistPaginate = await client.Playlists.GetItems(playlistId);

        return await client.Paginate(playlistPaginate)
            .Select(o => o.Track)
            .OfType<FullTrack>()
            .Select(Map)
            .ToListAsync();
    }

    public async Task<List<TrackInfo>> GetFromLibraryAsync()
    {
        ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync();

        Paging<SavedTrack> libPaginate = await client.Library.GetTracks();

        return await client.Paginate(libPaginate)
            .Select(o => o.Track)
            .Select(Map)
            .ToListAsync();
    }

    private TrackInfo Map(FullTrack track)
    {
        return new TrackInfo
        {
            Id = track.Id,
            Uri = track.Uri,
            Title = track.Name,
            AlbumName = track.Album.Name,
            DiscNumber = track.DiscNumber,
            TrackNumber = track.TrackNumber,
            Artists = track.Artists.Select(o => o.Name).ToList()
        };
    }
}
