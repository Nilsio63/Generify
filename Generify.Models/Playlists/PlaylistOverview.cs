namespace Generify.Models.Playlists;

public class PlaylistOverview
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public PlaylistDefinition Definition { get; set; } = new();
}
