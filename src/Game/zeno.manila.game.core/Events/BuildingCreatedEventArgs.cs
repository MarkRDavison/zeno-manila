namespace zeno.manila.game.core.Events;

public sealed class BuildingCreatedEventArgs : EventArgs
{

    public BuildingCreatedEventArgs(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }
    public bool CreatedManually { get; set; }
}
