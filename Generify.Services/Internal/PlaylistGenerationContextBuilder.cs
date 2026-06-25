using Generify.External.Abstractions.Models;
using Generify.External.Abstractions.Services;
using Generify.Models.Enums;
using Generify.Models.Playlists;
using Generify.Services.Internal.Interfaces;
using Generify.Services.Internal.Models;

namespace Generify.Services.Internal;

public class PlaylistGenerationContextBuilder(
    IPlaylistInfoService playlistInfoService,
    ITrackInfoService trackInfoService)
    : IPlaylistGenerationContextBuilder
{
    public async Task<PlaylistGenerationContext> CreateContextAsync(PlaylistDefinition playlistDefinition)
    {
        List<TrackInfo> sourceTracks = await GetFromSourcesAsync(playlistDefinition);

        sourceTracks = Sort(playlistDefinition, sourceTracks);

        PlaylistInfo targetPlaylist = await playlistInfoService.GetPlaylistInfoAsync(playlistDefinition.TargetPlaylistId)
            ?? throw new KeyNotFoundException($"Could not find playlist with id {playlistDefinition.TargetPlaylistId}");

        List<TrackInfo> targetTracks = await trackInfoService.GetByPlaylistIdAsync(targetPlaylist.Id);

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
            .Select(async (o, _, _) => new
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
        return await trackInfoService.GetByAlbumIdAsync(source.SourceId);
    }

    private async Task<List<TrackInfo>> GetFromArtistAsync(PlaylistSource source)
    {
        return await trackInfoService.GetByArtistIdAsync(source.SourceId);
    }

    private async Task<List<TrackInfo>> GetFromLibraryAsync()
    {
        return await trackInfoService.GetFromLibraryAsync();
    }

    private async Task<List<TrackInfo>> GetFromPlaylistAsync(PlaylistSource source)
    {
        return await trackInfoService.GetByPlaylistIdAsync(source.SourceId);
    }

    private async Task<List<TrackInfo>> GetFromTrackAsync(PlaylistSource source)
    {
        TrackInfo? track = await trackInfoService.GetByIdAsync(source.SourceId);

        if (track is not null)
        {
            return [track];
        }

        return [];
    }
}
