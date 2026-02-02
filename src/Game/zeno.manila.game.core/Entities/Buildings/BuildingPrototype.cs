namespace zeno.manila.game.core.Entities.Buildings;

public sealed class BuildingPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public HashSet<TileType> ValidTiles { get; set; } = [];
    public Dictionary<string, int> ActiveResources { get; set; } = [];
}
