using Generify.External.Abstractions.Models;

namespace Generify.External.Abstractions.Services;

public interface IPlaylistInfoService
{
    Task<PlaylistInfo?> GetPlaylistInfoAsync(string playlistId);
}
