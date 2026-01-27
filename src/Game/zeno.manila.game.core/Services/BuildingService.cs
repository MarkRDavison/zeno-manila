namespace zeno.manila.game.core.Services;

internal sealed class BuildingService : IBuildingService
{
    private readonly ManilaGameData _data;

    public BuildingService(ManilaGameData data)
    {
        _data = data;
    }

    public bool IsBuildingModeActive { get; private set; }

    public bool CanPlaceActiveBuildingAtTile(int x, int y)
    {
        throw new NotImplementedException();
    }

    public bool PlaceActiveBuildingAtTile(int x, int y)
    {
        throw new NotImplementedException();
    }

    public void SetBuildingModeActiveState(bool active)
    {
        throw new NotImplementedException();
    }

    public void SetBuildingType(string buildingType)
    {
        throw new NotImplementedException();
    }
}
