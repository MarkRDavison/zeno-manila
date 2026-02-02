namespace zeno.manila.game.core.Services.Contracts;

public interface ITeamService
{
    int NumberOfTeams { get; }
    string GetTeamName(int teamNumber);
    Color GetTeamColor(int teamNumber);
    void SetResourceAmount(int teamNumber, string resource, int amount);
    int GetResourceAmount(int teamNumber, string resource);
    bool IsTeamPlayable(int teamNumber);
}
