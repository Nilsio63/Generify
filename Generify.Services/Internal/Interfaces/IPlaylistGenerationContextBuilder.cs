using Generify.Models.Playlists;
using Generify.Services.Internal.Models;

namespace Generify.Services.Internal.Interfaces;

public interface IPlaylistGenerationContextBuilder
{
    Task<PlaylistGenerationContext> CreateContextAsync(PlaylistDefinition playlistDefinition);
}
