namespace zeno.manila.game.core.Entities.Buildings;

public sealed class BuildingPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public HashSet<string> CanRecruit { get; set; } = [];
    public HashSet<TileType> ValidTiles { get; set; } = [];
    public Dictionary<string, int> ActiveResources { get; set; } = []; // This can be negative
    public Dictionary<string, int> Cost { get; set; } = [];
    public Dictionary<string, int> Production { get; set; } = [];
}
