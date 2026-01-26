namespace zeno.manila.game.core.Components;

public sealed class CityComponent
{
    public Guid RootCityEntityId { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int TeamNumber { get; set; }
    public long Population { get; set; }
}
