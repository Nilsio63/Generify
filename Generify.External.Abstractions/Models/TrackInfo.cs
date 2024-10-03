using System.Collections.Generic;

namespace Generify.External.Abstractions.Models;

public class TrackInfo
{
    public string Id { get; set; }
    public string Uri { get; set; }
    public string Title { get; set; }
    public string AlbumName { get; set; }
    public int DiscNumber { get; set; }
    public int TrackNumber { get; set; }

    public List<string> Artists { get; set; }
}
