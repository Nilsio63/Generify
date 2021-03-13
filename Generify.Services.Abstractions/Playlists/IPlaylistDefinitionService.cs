using Generify.Models.Playlists;
using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Playlists
{
    public interface IPlaylistDefinitionService
    {
        Task ExecuteGenerationAsync(string playlistDefinitionId);
        Task SaveAsync(PlaylistDefinition playlistDefinition);
    }
}