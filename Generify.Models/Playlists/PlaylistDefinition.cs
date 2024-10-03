using Generify.Models.Management;
using System.Collections.Generic;

namespace Generify.Models.Playlists;

public class PlaylistDefinition : Entity
{
    public string TargetPlaylistId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = new();
    public List<PlaylistSource> PlaylistSources { get; set; } = [];
    public List<OrderInstruction> OrderInstructions { get; set; } = [];
}
