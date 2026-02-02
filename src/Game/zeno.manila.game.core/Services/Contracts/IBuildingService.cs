namespace zeno.manila.game.core.Services.Contracts;

public interface IBuildingService
{
    bool IsBuildingModeActive { get; }
    void SetBuildingModeActiveState(bool active);
    void SetBuildingType(string buildingType);
    bool CanPlaceActiveBuildingAtTile(int x, int y, int teamNumber);
    bool PlaceActiveBuildingAtTile(int x, int y, int teamNumber);

    EventHandler OnBuildingModeChanged { get; set; }
    EventHandler<BuildingCreatedEventArgs> OnBuildingCreated { get; set; }
}
