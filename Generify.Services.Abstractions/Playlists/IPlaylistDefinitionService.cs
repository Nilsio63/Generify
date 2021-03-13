using System.Threading.Tasks;

namespace Generify.Services.Abstractions.Playlists
{
    public interface IPlaylistDefinitionService
    {
        Task ExecuteGenerationAsync(string playlistDefinitionId);
    }
}