namespace Generify.Models.Playlists
{
    public class PlaylistOverview
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGenerating { get; set; }
        public string ErrorMessage { get; set; }

        public PlaylistDefinition Definition { get; set; }
    }
}
