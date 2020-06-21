using Generify.Models.Management;
using System.Collections.Generic;

namespace Generify.Models.Playlists
{
    public class PlaylistDefinition : Entity
    {
        public string TargetPlaylistId { get; set; }
        public bool IncludeSongsFromLibrary { get; set; }

        public User User { get; set; }
        public List<PlaylistSource> PlaylistSources { get; set; }
        public List<OrderInstruction> OrderInstructions { get; set; }
    }
}
