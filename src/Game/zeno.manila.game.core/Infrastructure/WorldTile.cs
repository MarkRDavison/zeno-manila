namespace zeno.manila.game.core.Infrastructure;

public struct WorldTile
{
    public WorldTile()
    {
        TileType = TileType.Unset;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public readonly Color Color => TileType switch
    {
        TileType.Land => Color.Green,
        TileType.Water => Color.Blue,
        TileType.Edge => Color.Black,
        TileType.Unset => Color.Magenta,
        _ => Color.Magenta
    };

    public TileType TileType { get; set; }
    public int OwningTeam { get; set; }
}
