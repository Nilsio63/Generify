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

            List<FullTrack> sourceTracks = await GetFromSources(playlistDefinition, client);

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

        private async Task<List<FullTrack>> GetFromSources(PlaylistDefinition playlistDefinition, ISpotifyClient client)
        {
            List<FullTrack> all = await playlistDefinition.PlaylistSources
                .ToAsyncEnumerable()
                .SelectAwait(async o => await GetFromSource(o, client))
                .SelectMany(o => o.ToAsyncEnumerable())
                .ToListAsync();

            return all
                .DistinctBy(o => o.Id)
                .ToList();
        }

        private async Task<List<FullTrack>> GetFromSource(PlaylistSource source, ISpotifyClient client)
        {
            return source.SourceType switch
            {
                SourceType.Library => await GetFromLibrary(client),
                SourceType.Album => await GetFromAlbum(source, client),
                _ => throw new NotSupportedException($"Source type '{source.SourceType}' is not supported!"),
            };
        }

        private static async Task<List<FullTrack>> GetFromLibrary(ISpotifyClient client)
        {
            Paging<SavedTrack> libPaginate = await client.Library.GetTracks();

            return await client.Paginate(libPaginate)
                .Select(o => o.Track)
                .ToListAsync();
        }

        private static async Task<List<FullTrack>> GetFromAlbum(PlaylistSource source, ISpotifyClient client)
        {
            Paging<SimpleTrack> albumPaginate = await client.Albums.GetTracks(source.SourceId);

            List<SimpleTrack> tracksFromAlbum = await client.Paginate(albumPaginate).ToListAsync();

            TracksResponse res = await client.Tracks.GetSeveral(new TracksRequest(tracksFromAlbum.Select(o => o.Id).ToList()));

            return res.Tracks;
        }
    }
}
