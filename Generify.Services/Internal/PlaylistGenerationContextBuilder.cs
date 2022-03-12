using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.Models.Enums;
using Generify.Models.Playlists;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generify.Services.Internal
{
    public class PlaylistGenerationContextBuilder : IPlaylistGenerationContextBuilder
    {
        private readonly IPlaylistInfoService _playlistInfoService;
        private readonly ITrackInfoService _trackInfoService;

        public PlaylistGenerationContextBuilder(IPlaylistInfoService playlistInfoService,
            ITrackInfoService trackInfoService)
        {
            _playlistInfoService = playlistInfoService;
            _trackInfoService = trackInfoService;
        }

        public async Task<PlaylistGenerationContext> CreateContextAsync(PlaylistDefinition playlistDefinition)
        {
            List<TrackInfo> sourceTracks = await GetFromSourcesAsync(playlistDefinition);

            sourceTracks = Sort(playlistDefinition, sourceTracks);

            PlaylistInfo targetPlaylist = await _playlistInfoService.GetPlaylistInfoAsync(playlistDefinition.TargetPlaylistId);

            List<TrackInfo> targetTracks = await _trackInfoService.GetByPlaylistIdAsync(targetPlaylist.Id);

            return new PlaylistGenerationContext
            {
                TargetPlaylist = targetPlaylist,
                SourceTracks = sourceTracks,
                TargetTracks = targetTracks
            };
        }

        private List<TrackInfo> Sort(PlaylistDefinition playlistDefinition, IEnumerable<TrackInfo> trackList)
        {
            foreach (OrderInstruction item in playlistDefinition.OrderInstructions)
            {
                switch (item.OrderType)
                {
                    case PlaylistOrderType.AlbumName:
                        trackList = GenericSort(trackList, o => o.AlbumName, item.OrderDirection);
                        break;
                    case PlaylistOrderType.ArtistName:
                        trackList = GenericSort(trackList, o => o.Artists.First(), item.OrderDirection);
                        break;
                    case PlaylistOrderType.Title:
                        trackList = GenericSort(trackList, o => o.Title, item.OrderDirection);
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

        private IEnumerable<TrackInfo> GenericSort<T>(IEnumerable<TrackInfo> trackList, Func<TrackInfo, T> sortSelector, PlaylistOrderDirection orderDirection)
        {
            if (trackList is IOrderedEnumerable<TrackInfo> ordered)
            {
                return orderDirection == PlaylistOrderDirection.Ascending
                    ? ordered.ThenBy(sortSelector)
                    : ordered.ThenByDescending(sortSelector);
            }
            else
            {
                return orderDirection == PlaylistOrderDirection.Ascending
                    ? trackList.OrderBy(sortSelector)
                    : trackList.OrderByDescending(sortSelector);
            }
        }

        private async Task<List<TrackInfo>> GetFromSourcesAsync(PlaylistDefinition playlistDefinition)
        {
            var sourceList = await playlistDefinition.PlaylistSources
                .ToAsyncEnumerable()
                .OrderBy(o => o.OrderNr)
                .SelectAwait(async o => new
                {
                    o.InclusionType,
                    Tracks = await GetFromSourceAsync(o)
                })
                .ToListAsync();

            IEnumerable<TrackInfo> all = new List<TrackInfo>();

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

        private async Task<List<TrackInfo>> GetFromSourceAsync(PlaylistSource source)
        {
            return source.SourceType switch
            {
                SourceType.Album => await GetFromAlbumAsync(source),
                SourceType.Artist => await GetFromArtistAsync(source),
                SourceType.Library => await GetFromLibraryAsync(),
                SourceType.Playlist => await GetFromPlaylistAsync(source),
                SourceType.Track => await GetFromTrackAsync(source),
                _ => throw new NotSupportedException($"Source type '{source.SourceType}' is not supported!"),
            };
        }

        private async Task<List<TrackInfo>> GetFromAlbumAsync(PlaylistSource source)
        {
            return await _trackInfoService.GetByAlbumIdAsync(source.SourceId);
        }

        private async Task<List<TrackInfo>> GetFromArtistAsync(PlaylistSource source)
        {
            return await _trackInfoService.GetByArtistIdAsync(source.SourceId);
        }

        private async Task<List<TrackInfo>> GetFromLibraryAsync()
        {
            return await _trackInfoService.GetFromLibraryAsync();
        }

        private async Task<List<TrackInfo>> GetFromPlaylistAsync(PlaylistSource source)
        {
            return await _trackInfoService.GetByPlaylistIdAsync(source.SourceId);
        }

        private async Task<List<TrackInfo>> GetFromTrackAsync(PlaylistSource source)
        {
            TrackInfo track = await _trackInfoService.GetByIdAsync(source.SourceId);

            return new List<TrackInfo> { track };
        }
    }
}
