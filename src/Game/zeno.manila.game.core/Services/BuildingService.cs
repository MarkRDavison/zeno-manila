namespace zeno.manila.game.core.Services;

internal sealed class BuildingService : IBuildingService
{
    private readonly ManilaGameData _data;

    private string _activeAuildingType = string.Empty;

    public BuildingService(ManilaGameData data)
    {
        _data = data;
    }

    public bool IsBuildingModeActive { get; private set; }

    public bool CanPlaceActiveBuildingAtTile(int x, int y)
    {
        return false;
    }

    public bool PlaceActiveBuildingAtTile(int x, int y)
    {
        return false;
    }

    public void SetBuildingModeActiveState(bool active)
    {
        Console.WriteLine("SetBuildingModeActiveStateL {0}", active);
        IsBuildingModeActive = active;
    }

    public void SetBuildingType(string buildingType)
    {
        Console.WriteLine("SetBuildingType: {0}", buildingType);
        _activeAuildingType = buildingType;
    }
}
