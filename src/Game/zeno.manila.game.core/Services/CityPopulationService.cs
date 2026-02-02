namespace zeno.manila.game.core.Services;

internal sealed class CityPopulationService : ICityPopulationService
{
    private readonly ManilaGameData _data;
    private const int MinTurnsForSprawlIncrease = 3;

    public CityPopulationService(ManilaGameData data)
    {
        _data = data;
    }

    // TODO: Need a way to initialise this process, give city with no sprawl and X pop run until stable
    public void Update(float delta)
    {
        foreach (var city in _data.Cities)
        {
            HandleCitySprawl(city);
        }
    }

    public void UpdateEndRound()
    {
        foreach (var city in _data.Cities)
        {
            HandleCityPopGrowth(city);
            HandleCitySprawl(city);
        }
    }

    private void HandleCityPopGrowth(City city)
    {
        city.Population = (int)(((float)(city.Population)) * 1.05f);
    }

    public void Init()
    {
        foreach (var city in _data.Cities)
        {
            while (UpdateCitySprawl(city)) { }
        }
    }

    private bool UpdateCitySprawl(City city)
    {
        var expectedSprawlForPopulation = CalculateExpectedSprawlForPopulation(city);

        if (expectedSprawlForPopulation > city.SprawlCount)
        {
            var loc = FindLocationForNewCitySprawl(city);

            if (loc is not null && GetTile((int)loc.Value.X, (int)loc.Value.Y) is { } tile)
            {

                var sprawl = new CitySprawl
                {
                    X = (int)loc.Value.X,
                    Y = (int)loc.Value.Y
                };

                city.Sprawl.Add(sprawl);
                city.LastSprawlIncreaseTurnNumber = _data.CurrentTurnNumber;

                tile.OwningTeam = city.TeamNumber;
            }

            return true;

        }

        return false;
    }

    private void HandleCitySprawl(City city)
    {
        if (city.LastSprawlIncreaseTurnNumber + MinTurnsForSprawlIncrease > _data.CurrentTurnNumber)
        {
            return;
        }

        UpdateCitySprawl(city);
    }

    // TODO: This works, but very inorganic, expands like a square, change to circle?
    //       With fuzzy edges? More likely closer to the center?
    private Vector2? FindLocationForNewCitySprawl(City city)
    {
        int breakout = 100;
        int radius = 0;

        HashSet<Vector2> availableLocations = [];

        while (breakout > 0 && availableLocations.Count is 0)
        {
            var xPos = city.X;
            var yPos = city.Y;

            // N/S

            for (int x = -radius; x <= radius; x++)
            {
                var tempX = xPos + x;

                if (ValidateCoordinates(tempX, yPos - radius - 1, city.TeamNumber) is { } coordN)
                {
                    availableLocations.Add(coordN);
                }

                if (ValidateCoordinates(tempX, yPos + radius + 1, city.TeamNumber) is { } coordS)
                {
                    availableLocations.Add(coordS);
                }
            }

            // E/W

            for (int y = -radius; y <= radius; y++)
            {
                var tempY = yPos + y;

                if (ValidateCoordinates(xPos + radius + 1, tempY, city.TeamNumber) is { } coordE)
                {
                    availableLocations.Add(coordE);
                }

                if (ValidateCoordinates(xPos - radius - 1, tempY, city.TeamNumber) is { } coordW)
                {
                    availableLocations.Add(coordW);
                }
            }

            // Diagonals if radius > 1

            if (radius > 0)
            {
                if (ValidateCoordinates(xPos + radius, yPos + radius, city.TeamNumber) is { } cpp)
                {
                    availableLocations.Add(cpp);
                }

                if (ValidateCoordinates(xPos + radius, yPos - radius, city.TeamNumber) is { } cpn)
                {
                    availableLocations.Add(cpn);
                }

                if (ValidateCoordinates(xPos - radius, yPos - radius, city.TeamNumber) is { } cnn)
                {
                    availableLocations.Add(cnn);
                }

                if (ValidateCoordinates(xPos - radius, yPos + radius, city.TeamNumber) is { } cnp)
                {
                    availableLocations.Add(cnp);
                }
            }

            radius++;
            breakout--;
        }

        if (availableLocations.Count is 0)
        {
            return null;
        }

        return availableLocations.Skip(Random.Shared.Next(availableLocations.Count)).First();
    }

    private Vector2? ValidateCoordinates(int x, int y, int owningTeam)
    {
        if (GetTile(x, y) is { } tile && tile.OwningTeam is 0 && tile.TileType is TileType.Land &&
            ((GetTile(x + 1, y) is { } r && r.OwningTeam == owningTeam) ||
            (GetTile(x - 1, y) is { } l && l.OwningTeam == owningTeam) ||
            (GetTile(x, y - 1) is { } u && u.OwningTeam == owningTeam) ||
            (GetTile(x, y + 1) is { } d && d.OwningTeam == owningTeam)))
        {
            return new Vector2(x, y);
        }

        return null;
    }

    private WorldTile? GetTile(int x, int y)
    {
        if (x >= 0 && y >= 0 &&
            x < _data.WorldWidth && y < _data.WorldHeight)
        {
            return _data.Tiles[y][x];
        }

        return null;
    }

    private static int CalculateExpectedSprawlForPopulation(City city)
    {
        int pow = 0;

        var pop = city.Population;

        // TODO: Log?  
        while (pop >= 10)
        {
            pow++;
            pop /= 10;
        }

        return pow;
    }
}
