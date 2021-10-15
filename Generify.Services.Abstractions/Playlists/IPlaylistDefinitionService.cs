using Generify.Models.Playlists;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Playlists
{
    public interface IPlaylistDefinitionService
    {
        Task<List<PlaylistDefinition>> GetAllByUserIdAsync(string userId);
        Task<PlaylistDefinition> GetByIdForUserAsync(string playlistId, string userId);
        Task LoadDetailsAsync(PlaylistDefinition playlistDefinition);
        Task ExecuteGenerationAsync(string playlistDefinitionId);
        Task SaveAsync(PlaylistDefinition playlistDefinition);
        Task DeleteAsync(PlaylistDefinition playlistDefinition);
    }
}