namespace zeno.manila.game.core.Infrastructure;

public class WorldTile
{

    public int X { get; set; }
    public int Y { get; set; }
    public Color Color => TileType switch
    {
        TileType.Land => Color.Green,
        TileType.Water => Color.Blue,
        TileType.Shore => Color.Orange,
        TileType.Edge => Color.Black,
        TileType.Unset => Color.Magenta,
        _ => Color.Magenta
    };

    public TileType TileType { get; set; } = TileType.Unset;
    public int OwningTeam { get; set; }
}
