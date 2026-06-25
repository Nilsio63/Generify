using Generify.External.Abstractions.Models;

namespace Generify.Services.Internal.Models;

public class PlaylistGenerationContext
{
    public PlaylistInfo TargetPlaylist { get; set; } = new();
    public List<TrackInfo> SourceTracks { get; set; } = [];
    public List<TrackInfo> TargetTracks { get; set; } = [];
}
