using Generify.Models.Enums;

namespace Generify.Models.Playlists;

public class OrderInstruction : Entity
{
    public int OrderNr { get; set; }
    public PlaylistOrderType OrderType { get; set; }
    public PlaylistOrderDirection OrderDirection { get; set; }

    public PlaylistDefinition PlaylistDefinition { get; set; } = new();
}
