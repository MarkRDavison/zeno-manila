namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameData
{
    public int WorldWidth { get; set; }
    public int WorldHeight { get; set; }

    public List<List<WorldTile>> Tiles { get; set; } = [];

    public World World { get; set; } = new();
    public int CurrentTurnNumber { get; set; }
}
