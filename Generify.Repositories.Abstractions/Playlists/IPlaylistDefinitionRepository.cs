using Generify.Models.Playlists;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Repositories.Abstractions.Playlists
{
    public interface IPlaylistDefinitionRepository : IBaseRepository<PlaylistDefinition>
    {
        Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId);

        Task LoadDetailsAsync(PlaylistDefinition playlistDefinition);
    }
}
