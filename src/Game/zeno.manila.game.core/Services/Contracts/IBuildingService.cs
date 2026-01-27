namespace zeno.manila.game.core.Services.Contracts;

public interface IBuildingService
{
    bool IsBuildingModeActive { get; }
    void SetBuildingModeActiveState(bool active);
    void SetBuildingType(string buildingType);
    bool CanPlaceActiveBuildingAtTile(int x, int y);
    bool PlaceActiveBuildingAtTile(int x, int y);
}
