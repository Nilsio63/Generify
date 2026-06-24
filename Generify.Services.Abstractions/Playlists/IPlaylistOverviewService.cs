using Generify.Models.Playlists;

namespace Generify.Services.Abstractions.Playlists;

public interface IPlaylistOverviewService
{
    Task<List<PlaylistOverview>> GetAllByUserIdAsync(string userId);
}
