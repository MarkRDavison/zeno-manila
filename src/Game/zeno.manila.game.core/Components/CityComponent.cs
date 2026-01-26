namespace zeno.manila.game.core.Components;

public sealed class CityComponent
{
    public int X { get; set; }
    public int Y { get; set; }
    public int TeamNumber { get; set; }
    public long Population { get; set; }
    public int SprawlCount { get; set; }
    public int LastSprawlIncreaseTurnNumber { get; set; }
}
