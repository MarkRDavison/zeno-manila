namespace zeno.manila.game.core.Services;

internal sealed class BuildingService : IBuildingService
{
    private readonly ManilaGameData _data;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;

    private string _activeBuildingType = string.Empty;

    public BuildingService(
        ManilaGameData data,
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService)
    {
        _data = data;
        _buildingPrototypeService = buildingPrototypeService;
    }

    public bool IsBuildingModeActive { get; private set; }
    public bool HasSelectedBuilding => !string.IsNullOrEmpty(_activeBuildingType);
    public EventHandler OnBuildingModeChanged { get; set; } = default!;
    public EventHandler<BuildingCreatedEventArgs> OnBuildingCreated { get; set; } = default!;

    public bool CanPlaceActiveBuildingAtTile(int x, int y, int teamNumber)
    {
        if (!HasSelectedBuilding)
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

        var prototype = _buildingPrototypeService.GetPrototype(_activeBuildingType);

        if (!prototype.ValidTiles.Contains(tile.TileType))
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

        sprawl.RelatedEntity = _buildingPrototypeService.CreateEntity(_activeBuildingType);

        SetBuildingModeActiveState(false);
        OnBuildingCreated?.Invoke(this, new BuildingCreatedEventArgs(x, y)
        {
            CreatedManually = true
        });

        return true;
    }

    public void SetBuildingModeActiveState(bool active)
    {
        _activeBuildingType = string.Empty;
        IsBuildingModeActive = active;
        OnBuildingModeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetBuildingType(string buildingType)
    {
        _activeBuildingType = buildingType;
    }
}
