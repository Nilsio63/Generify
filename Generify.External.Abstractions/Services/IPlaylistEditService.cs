using Generify.External.Abstractions.Models;

namespace Generify.External.Abstractions.Services;

public interface IPlaylistEditService
{
    Task AddTracksToPlaylistAsync(string playlistId, IEnumerable<TrackInfo> tracks);
    Task RemoveTracksFromPlaylistAsync(string playlistId, IEnumerable<TrackInfo> tracks);
    Task ReorderTracksInPlaylistAsync(string playlistId, int startIndex, int insertBeforeIndex);
}
