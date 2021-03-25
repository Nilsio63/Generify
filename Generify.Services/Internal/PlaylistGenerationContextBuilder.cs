using Generify.Models.Enums;
using Generify.Models.Playlists;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;
using MoreLinq;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Internal
{
    public class PlaylistGenerationContextBuilder : IPlaylistGenerationContextBuilder
    {
        private readonly ISpotifyClientFactory _spotifyClientFactory;

        public PlaylistGenerationContextBuilder(ISpotifyClientFactory spotifyClientFactory)
        {
            _spotifyClientFactory = spotifyClientFactory;
        }

        public async Task<PlaylistGenerationContext> CreateContextAsync(PlaylistDefinition playlistDefinition)
        {
            ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync(playlistDefinition.User.RefreshToken);

            PrivateUser user = await client.UserProfile.Current();

            List<FullTrack> sourceTracks = await GetFromSourcesAsync(playlistDefinition, client);

            sourceTracks = Sort(playlistDefinition, sourceTracks);

            FullPlaylist targetPlaylist = await client.Playlists.Get(playlistDefinition.TargetPlaylistId);

            List<FullTrack> targetTracks = await client.Paginate(targetPlaylist.Tracks)
                .Select(o => o.Track)
                .OfType<FullTrack>()
                .ToListAsync();

            return new PlaylistGenerationContext
            {
                Client = client,
                TargetPlaylist = targetPlaylist,
                SourceTracks = sourceTracks,
                TargetTracks = targetTracks
            };
        }

        private List<FullTrack> Sort(PlaylistDefinition playlistDefinition, IEnumerable<FullTrack> trackList)
        {
            foreach (OrderInstruction item in playlistDefinition.OrderInstructions)
            {
                switch (item.OrderType)
                {
                    case PlaylistOrderType.AlbumName:
                        trackList = GenericSort(trackList, o => o.Album.Name, item.OrderDirection);
                        break;
                    case PlaylistOrderType.ArtistName:
                        trackList = GenericSort(trackList, o => o.Artists.First().Name, item.OrderDirection);
                        break;
                    case PlaylistOrderType.Title:
                        trackList = GenericSort(trackList, o => o.Name, item.OrderDirection);
                        break;
                    case PlaylistOrderType.TrackNr:
                        trackList = GenericSort(trackList, o => o.DiscNumber, item.OrderDirection);
                        trackList = GenericSort(trackList, o => o.TrackNumber, item.OrderDirection);
                        break;
                    default:
                        throw new NotSupportedException($"Order type '{item.OrderType}' is not supported!");
                }
            }

            return trackList.ToList();
        }

        private IEnumerable<FullTrack> GenericSort<T>(IEnumerable<FullTrack> trackList, Func<FullTrack, T> sortSelector, PlaylistOrderDirection orderDirection)
        {
            OrderByDirection direction = orderDirection == PlaylistOrderDirection.Descending
                ? OrderByDirection.Descending
                : OrderByDirection.Ascending;

            if (trackList is IOrderedEnumerable<FullTrack> ordered)
            {
                return ordered.ThenBy(sortSelector, direction);
            }
            else
            {
                return trackList.OrderBy(sortSelector, direction);
            }
        }

        private async Task<List<FullTrack>> GetFromSourcesAsync(PlaylistDefinition playlistDefinition, ISpotifyClient client)
        {
            var sourceList = await playlistDefinition.PlaylistSources
                .ToAsyncEnumerable()
                .OrderBy(o => o.OrderNr)
                .SelectAwait(async o => new
                {
                    o.InclusionType,
                    Tracks = await GetFromSourceAsync(o, client)
                })
                .ToListAsync();

            IEnumerable<FullTrack> all = new List<FullTrack>();

            foreach (var source in sourceList)
            {
                all = source.InclusionType switch
                {
                    InclusionType.Include => all.Concat(source.Tracks),
                    InclusionType.Exclude => all.Except(source.Tracks),
                    _ => throw new NotSupportedException($"Inclusion type '{source.InclusionType}' is not supported!"),
                };
            }

            return all
                .DistinctBy(o => o.Id)
                .ToList();
        }

        private async Task<List<FullTrack>> GetFromSourceAsync(PlaylistSource source, ISpotifyClient client)
        {
            return source.SourceType switch
            {
                SourceType.Album => await GetFromAlbumAsync(source, client),
                SourceType.Artist => await GetFromArtistAsync(source, client),
                SourceType.Library => await GetFromLibraryAsync(client),
                SourceType.Playlist => await GetFromPlaylistAsync(source, client),
                SourceType.Track => await GetFromTrackAsync(source, client),
                _ => throw new NotSupportedException($"Source type '{source.SourceType}' is not supported!"),
            };
        }

        private async Task<List<FullTrack>> GetFromAlbumAsync(PlaylistSource source, ISpotifyClient client)
        {
            Paging<SimpleTrack> albumPaginate = await client.Albums.GetTracks(source.SourceId);

            List<SimpleTrack> tracksFromAlbum = await client.Paginate(albumPaginate).ToListAsync();

            return await tracksFromAlbum
                .Batch(50)
                .ToAsyncEnumerable()
                .SelectAwait(async o => await client.Tracks.GetSeveral(new TracksRequest(o.Select(p => p.Id).ToList())))
                .SelectMany(o => o.Tracks.ToAsyncEnumerable())
                .ToListAsync();
        }

        private async Task<List<FullTrack>> GetFromArtistAsync(PlaylistSource source, ISpotifyClient client)
        {
            Paging<SimpleAlbum> albumPaginate = await client.Artists.GetAlbums(source.SourceId);

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
                .ToListAsync();
        }

        private async Task<List<FullTrack>> GetFromLibraryAsync(ISpotifyClient client)
        {
            Paging<SavedTrack> libPaginate = await client.Library.GetTracks();

            return await client.Paginate(libPaginate)
                .Select(o => o.Track)
                .ToListAsync();
        }

        private async Task<List<FullTrack>> GetFromPlaylistAsync(PlaylistSource source, ISpotifyClient client)
        {
            Paging<PlaylistTrack<IPlayableItem>> playlistPaginate = await client.Playlists.GetItems(source.SourceId);

            return await client.Paginate(playlistPaginate)
                .Select(o => o.Track)
                .OfType<FullTrack>()
                .ToListAsync();
        }

        private async Task<List<FullTrack>> GetFromTrackAsync(PlaylistSource source, ISpotifyClient client)
        {
            FullTrack track = await client.Tracks.Get(source.SourceId);

            return new List<FullTrack> { track };
        }
    }
}
