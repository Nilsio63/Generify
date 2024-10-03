using Generify.Models.Playlists;
using System.Collections.Generic;

namespace Generify.Models.Management;

public class User : Entity
{
    public string SpotifyId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    public List<PlaylistDefinition> PlaylistDefinitions { get; set; } = [];
}
