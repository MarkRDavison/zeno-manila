namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameData
{
    public int WorldWidth { get; set; }
    public int WorldHeight { get; set; }

    public List<List<WorldTile>> Tiles { get; set; } = [];

    public int CurrentTurnNumber { get; set; }

    public List<City> Cities { get; } = [];
    public List<Building> Buildings { get; } = [];
    public List<MilitaryUnit> MilitaryUnits { get; } = [];

    public WorldTile GetSafeTile(int x, int y)
    {
        return Tiles[y][x];
    }

    public WorldTile? GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= WorldWidth || y >= WorldHeight)
        {
            return null;
        }

        return GetSafeTile(x, y);
    }

    public City? GetCityAtTile(int x, int y)
    {
        return Cities.FirstOrDefault(_ => _.X == x && _.Y == y);
    }

    public (City?, CitySprawl?) GetCityAndSprawlAtTile(int x, int y)
    {
        var city = Cities.FirstOrDefault(c => c.Sprawl.FirstOrDefault(_ => _.X == x && _.Y == y) is not null);

        var sprawl = city?.Sprawl.FirstOrDefault(_ => _.X == x && _.Y == y);

        return (city, sprawl);
    }
}
