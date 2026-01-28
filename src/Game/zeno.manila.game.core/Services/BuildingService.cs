namespace zeno.manila.game.core.Services;

internal sealed class BuildingService : IBuildingService
{
    private readonly ManilaGameData _data;

    private string _activeBuildingType = "MilitaryBase";

    public BuildingService(ManilaGameData data)
    {
        _data = data;
    }

    public bool IsBuildingModeActive { get; private set; }

    public bool CanPlaceActiveBuildingAtTile(int x, int y, int teamNumber)
    {
        var tile = _data.GetTile(x, y);

        if (tile is null)
        {
            return false;
        }

        if (tile.OwningTeam != teamNumber)
        {
            return false;
        }

        // TODO: Different for different buildings
        if (tile.TileType != TileType.Land)
        {
            return false;
        }

        var (city, sprawl) = _data.GetCityAndSprawlAtTile(x, y);

        if (city is null || sprawl is null)
        {
            return false;
        }

        if (sprawl.RelatedEntity is not null)
        {
            return false;
        }

        return true;
    }

    public bool PlaceActiveBuildingAtTile(int x, int y, int teamNumber)
    {
        if (!CanPlaceActiveBuildingAtTile(x, y, teamNumber))
        {
            return false;
        }

        var (city, sprawl) = _data.GetCityAndSprawlAtTile(x, y);

        if (city is null || sprawl is null)
        {
            return false;
        }

        sprawl.RelatedEntity = new MilitaryBase
        {
            Id = Guid.NewGuid()
        };

        Console.WriteLine("Added {0} at {1},{2}", _activeBuildingType, x, y);

        return true;
    }

    public void SetBuildingModeActiveState(bool active)
    {
        Console.WriteLine("SetBuildingModeActiveStateL {0}", active);
        IsBuildingModeActive = active;
    }

    public void SetBuildingType(string buildingType)
    {
        Console.WriteLine("SetBuildingType: {0}", buildingType);
        _activeBuildingType = buildingType;
    }
}
