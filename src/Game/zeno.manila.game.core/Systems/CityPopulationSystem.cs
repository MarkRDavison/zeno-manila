namespace zeno.manila.game.core.Systems;

internal sealed class CityPopulationSystem : WorldSystem
{
    private readonly ManilaGameData _data;
    private const int MinTurnsForSprawlIncrease = 3;

    public CityPopulationSystem(ManilaGameData data)
    {
        _data = data;
    }

    public override void Update(World world, float delta)
    {
        foreach (var e in world.GetWithAll<CityComponent>())
        {
            var city = e.Get<CityComponent>();

            if (city.LastSprawlIncreaseTurnNumber + MinTurnsForSprawlIncrease > _data.CurrentTurnNumber)
            {
                continue;
            }

            var expectedSprawlForPopulation = CalculateExpectedSprawlForPopulation(city);

            if (expectedSprawlForPopulation > city.SprawlCount)
            {
                var (x, y) = FindLocationForNewCitySprawl(city);

                city.SprawlCount++;
                city.LastSprawlIncreaseTurnNumber = _data.CurrentTurnNumber;

                var sprawl = new CitySprawlComponent { 
                    X = x,
                    Y = y,
                    CityEntityId = e.Id,
                    TeamNumber = city.TeamNumber
                };
                world.Create(sprawl);
            }
        }
    }

    private (int x, int y) FindLocationForNewCitySprawl(CityComponent city)
    {


        throw new InvalidOperationException();
    }

    private static int CalculateExpectedSprawlForPopulation(CityComponent city)
    {
        int pow = 0;

        var pop = city.Population;

        while (pop >= 10)
        {
            pow++;
            pop /= 10;
        }

        return pow;
    }

    private List<(int x, int y)> GetValidCellsForSprawlRadius(int x, int y, int radius)
    {
        var cells = new List<(int x, int y)>();

        return cells;
    }
}
