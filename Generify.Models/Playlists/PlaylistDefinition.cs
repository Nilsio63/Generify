using Generify.Models.Management;
using System;
using System.Collections.Generic;

namespace Generify.Models.Playlists;

public class PlaylistDefinition : Entity
{
    public string TargetPlaylistId { get; set; } = string.Empty;
    public Guid UserId { get; set; }

    public User User { get; set; } = new();
    public List<PlaylistSource> PlaylistSources { get; set; } = [];
    public List<OrderInstruction> OrderInstructions { get; set; } = [];
}
