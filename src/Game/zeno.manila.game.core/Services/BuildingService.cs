namespace zeno.manila.game.core.Services;

internal sealed class BuildingService : IBuildingService
{
    private readonly ManilaGameData _data;

    private string _activeBuildingType = string.Empty;

    public BuildingService(ManilaGameData data)
    {
        _data = data;
    }

    public bool IsBuildingModeActive { get; private set; }
    public EventHandler OnBuildingModeChanged { get; set; } = default!;

    public bool CanPlaceActiveBuildingAtTile(int x, int y, int teamNumber)
    {
        if (string.IsNullOrEmpty(_activeBuildingType))
        {
            return false;
        }

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

        // TODO: Prototype system
        sprawl.RelatedEntity = _activeBuildingType switch
        {
            "MilitaryBase" => new MilitaryBase(),
            "PowerPlant" => new PowerPlant(),
            _ => throw new InvalidOperationException($"Cannot create building from type {_activeBuildingType}")
        };

        SetBuildingModeActiveState(false);
        return sprawl.RelatedEntity is not null;
    }

    public void SetBuildingModeActiveState(bool active)
    {
        IsBuildingModeActive = active;
        OnBuildingModeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetBuildingType(string buildingType)
    {
        _activeBuildingType = buildingType;
    }
}
