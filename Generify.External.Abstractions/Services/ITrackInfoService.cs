using Generify.External.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.External.Abstractions.Services;

public interface ITrackInfoService
{
    Task<TrackInfo?> GetByIdAsync(string trackId);
    Task<List<TrackInfo>> GetByAlbumIdAsync(string albumId);
    Task<List<TrackInfo>> GetByArtistIdAsync(string artistId);
    Task<List<TrackInfo>> GetByPlaylistIdAsync(string playlistId);
    Task<List<TrackInfo>> GetFromLibraryAsync();
}
