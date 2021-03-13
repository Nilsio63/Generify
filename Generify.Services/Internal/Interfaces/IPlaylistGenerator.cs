using Generify.Models.Playlists;
using System.Threading.Tasks;

namespace Generify.Services.Internal.Interfaces
{
    public interface IPlaylistGenerator
    {
        Task ExecuteGenerationAsync(PlaylistDefinition playlistDefinition);
    }
}
