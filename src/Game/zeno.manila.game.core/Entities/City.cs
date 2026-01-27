namespace zeno.manila.game.core.Entities;

public sealed class City
{
    public int X { get; set; }
    public int Y { get; set; }
    public int TeamNumber { get; set; }
    public long Population { get; set; }
    public int SprawlCount => Sprawl.Count;
    public int LastSprawlIncreaseTurnNumber { get; set; }
    public List<CitySprawl> Sprawl { get; } = [];
}
