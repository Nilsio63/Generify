using Generify.Models.Enums;
using Generify.Models.Playlists;
using Generify.Services.Abstractions.Management;
using MoreLinq;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Playlists
{
    public interface ISpotifyClientFactory
    {
        Task<ISpotifyClient> CreateClientAsync(string refreshToken);
    }

    public class SpotifyClientFactory : ISpotifyClientFactory
    {
        private readonly IExternalAuthSettings _externalAuthSettings;

        public SpotifyClientFactory(IExternalAuthSettings externalAuthSettings)
        {
            _externalAuthSettings = externalAuthSettings;
        }

        public async Task<ISpotifyClient> CreateClientAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException($"'{nameof(refreshToken)}' cannot be null or whitespace", nameof(refreshToken));
            }

            AuthorizationCodeRefreshResponse codeRefreshResponse = await new OAuthClient().RequestToken(new AuthorizationCodeRefreshRequest(_externalAuthSettings.ClientId, _externalAuthSettings.ClientSecret, refreshToken));

            SpotifyClientConfig clientConfig = SpotifyClientConfig
                .CreateDefault()
                .WithToken(codeRefreshResponse.AccessToken);

            return new SpotifyClient(clientConfig);
        }
    }

    public interface IPlaylistGeneratorService
    {
        Task ExecuteGenerationAsync(PlaylistDefinition playlistDefinition);
    }

    public class PlaylistGeneratorService : IPlaylistGeneratorService
    {
        private readonly ISpotifyClientFactory _spotifyClientFactory;

        public PlaylistGeneratorService(ISpotifyClientFactory spotifyClientFactory)
        {
            _spotifyClientFactory = spotifyClientFactory;
        }

        public async Task ExecuteGenerationAsync(PlaylistDefinition playlistDefinition)
        {
            ISpotifyClient client = await _spotifyClientFactory.CreateClientAsync(playlistDefinition.User.RefreshToken);

            PrivateUser user = await client.UserProfile.Current();

            List<FullTrack> sourceTracks = await GetFromSources(playlistDefinition, client);

            sourceTracks = Sort(playlistDefinition, sourceTracks);

            FullPlaylist targetPlaylist = await client.Playlists.Get(playlistDefinition.TargetPlaylistId);

            string snapShotId = targetPlaylist.SnapshotId;

            List<FullTrack> targetTracks = await client.Paginate(targetPlaylist.Tracks)
                .Select(o => o.Track)
                .OfType<FullTrack>()
                .ToListAsync();

            FullTrack[] toDelete = targetTracks
                .Where(t => !sourceTracks.Any(s => s.Id == t.Id))
                .ToArray();

            FullTrack[] toAdd = sourceTracks
                .Where(s => !targetTracks.Any(t => t.Id == s.Id))
                .ToArray();

            foreach (IEnumerable<FullTrack> batch in toDelete.Batch(100))
            {
                PlaylistRemoveItemsRequest req = new PlaylistRemoveItemsRequest
                {
                    Tracks = batch.Select(o => new PlaylistRemoveItemsRequest.Item { Uri = o.Uri }).ToList()
                };

                SnapshotResponse res = await client.Playlists.RemoveItems(playlistDefinition.TargetPlaylistId, req);

                snapShotId = res.SnapshotId;
            }

            foreach (IEnumerable<FullTrack> batch in toAdd.Batch(100))
            {
                SnapshotResponse res = await client.Playlists.AddItems(playlistDefinition.TargetPlaylistId, new PlaylistAddItemsRequest(batch.Select(o => o.Uri).ToList()));

                snapShotId = res.SnapshotId;
            }

            targetTracks = targetTracks
                .Except(toDelete)
                .Concat(toAdd)
                .ToList();

            int counter = 0;

            while (true)
            {
                var reorders = targetTracks
                    .Select((t, i) =>
                    {
                        var newIndex = sourceTracks.FindIndex(o => o.Id == t.Id);

                        return new
                        {
                            TrackUri = t.Uri,
                            OldIndex = i,
                            NewIndex = newIndex
                        };
                    })
                    .Where(o => o.OldIndex != o.NewIndex)
                    .OrderBy(o => Math.Min(o.NewIndex, o.OldIndex))
                    .ThenBy(o => o.NewIndex)
                    .ToArray();

                if (!reorders.Any())
                {
                    break;
                }

                var item = reorders.First();

                await client.Playlists.ReorderItems(targetPlaylist.Id, new PlaylistReorderItemsRequest(item.OldIndex, item.NewIndex)
                {
                    //SnapshotId = snapShotId
                });

                targetTracks = targetTracks
                    .Move(item.OldIndex, 1, item.NewIndex)
                    .ToList();

                counter++;
            }
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
                //.Take(15) // TODO
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
