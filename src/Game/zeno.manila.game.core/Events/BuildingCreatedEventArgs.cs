namespace zeno.manila.game.core.Events;

public sealed class BuildingCreatedEventArgs : EventArgs
{

    public BuildingCreatedEventArgs(int x, int y, string buildingType)
    {
        X = x;
        Y = y;
        BuildingType = buildingType;
    }

    public int X { get; }
    public int Y { get; }
    public string BuildingType { get; }
    public bool CreatedManually { get; set; }
}
