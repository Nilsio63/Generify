using System.Collections.Generic;

namespace Generify.External.Abstractions.Models;

public class TrackInfo
{
    public string Id { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string AlbumName { get; set; } = string.Empty;
    public int DiscNumber { get; set; }
    public int TrackNumber { get; set; }

    public List<string> Artists { get; set; } = [];
}
