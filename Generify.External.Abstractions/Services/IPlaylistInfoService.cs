using Generify.External.Abstractions.Models;
using System.Threading.Tasks;

namespace Generify.External.Abstractions.Services
{
    public interface IPlaylistInfoService
    {
        Task<PlaylistInfo> GetPlaylistInfoAsync(string playlistId);
    }
}
