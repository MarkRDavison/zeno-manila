namespace zeno.manila.game.core.Services;

internal sealed class TeamService : ITeamService
{
    private readonly IList<TeamInfo> _teams = [];

    public TeamService()
    {
        _teams =
        [
            new TeamInfo
            {
                IsPlayerTeam = true,
                Color = Color.Red,
                Name = "Team One"
            },
            new TeamInfo
            {
                Color = Color.Yellow,
                Name = "Team Two"
            },
            new TeamInfo
            {
                Color = Color.DarkPurple,
                Name = "Team Three"
            }
        ];
    }

    public int NumberOfTeams => _teams.Count;

    public string GetTeamName(int teamNumber) => _teams[teamNumber - 1].Name;

    public Color GetTeamColor(int teamNumber) => _teams[teamNumber - 1].Color;

    public void SetResourceAmount(int teamNumber, string resource, int amount)
    {
        var resources = _teams[teamNumber - 1].Resources;

        resources[resource] = amount;
    }

    public int GetResourceAmount(int teamNumber, string resource)
    {
        var resources = _teams[teamNumber - 1].Resources;

        if (resources.TryGetValue(resource, out var val))
        {
            return val;
        }

        return 0;
    }
    public bool IsTeamPlayable(int teamNumber) => _teams[teamNumber - 1].IsPlayerTeam;
}
