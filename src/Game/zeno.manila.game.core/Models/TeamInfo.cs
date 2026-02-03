namespace zeno.manila.game.core.Models;

public sealed class TeamInfo
{
    public bool IsPlayerTeam { get; set; }
    public string Name { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.White;
    public Dictionary<string, int> Resources { get; set; } = new()
    {
        { ManilaConstants.Resource_Credits, 1000 },
        { ManilaConstants.Resource_Food, 500 },
        { ManilaConstants.Resource_Fuel, 200 },
        { ManilaConstants.Resource_Electronics, 200 },
        { ManilaConstants.Resource_Materials, 200 }
    };
}
