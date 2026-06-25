using Generify.Models.Playlists;

namespace Generify.Services.Internal.Interfaces;

public interface IPlaylistGenerator
{
    Task ExecuteGenerationAsync(PlaylistDefinition playlistDefinition);
}
