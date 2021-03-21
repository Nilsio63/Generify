using SpotifyAPI.Web;
using System.Collections.Generic;

namespace Generify.Services.Internal.Models
{
    public class PlaylistGenerationContext
    {
        public ISpotifyClient Client { get; set; }
        public FullPlaylist TargetPlaylist { get; set; }
        public List<FullTrack> SourceTracks { get; set; }
        public List<FullTrack> TargetTracks { get; set; }
    }
}
