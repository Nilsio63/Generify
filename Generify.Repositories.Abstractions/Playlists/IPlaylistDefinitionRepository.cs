using Generify.Models.Playlists;

namespace Generify.Repositories.Abstractions.Playlists;

public interface IPlaylistDefinitionRepository : IBaseRepository<PlaylistDefinition>
{
    Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId);
    Task<PlaylistDefinition?> GetByIdForUserAsync(string playlistId, string userId);

    Task LoadDetailsAsync(PlaylistDefinition playlistDefinition);
}
