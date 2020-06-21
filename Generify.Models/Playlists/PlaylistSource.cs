using Generify.Models.Enums;

namespace Generify.Models.Playlists
{
    public class PlaylistSource : Entity
    {
        public string SourceId { get; set; }
        public SourceType SourceType { get; set; }
        public InclusionType InclusionType { get; set; }

        public PlaylistDefinition PlaylistDefinition { get; set; }
    }
}
