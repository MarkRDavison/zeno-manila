namespace zeno.manila.game.core.Models;

public sealed class TeamInfo
{
    public string Name { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.White;
    public Dictionary<string, int> Resources { get; set; } = new()
    {
        {"Credits", 1000}
    };
}
