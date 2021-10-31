using Generify.External.Abstractions.Models;
using System.Collections.Generic;

namespace Generify.Services.Internal.Models
{
    public class PlaylistGenerationContext
    {
        public PlaylistInfo TargetPlaylist { get; set; }
        public List<TrackInfo> SourceTracks { get; set; }
        public List<TrackInfo> TargetTracks { get; set; }
    }
}
