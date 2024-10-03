using Generify.Models.Playlists;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Playlists;

public interface IPlaylistOverviewService
{
    Task<List<PlaylistOverview>> GetAllByUserIdAsync(string userId);
}
