namespace zeno.manila.game.core.Services;

internal sealed class TurnService : ITurnService
{
    private int _currentTeamTurn;
    private readonly ITeamService _teamService;
    private readonly ManilaGameData _data;

    public TurnService(ITeamService teamService, ManilaGameData data)
    {
        _teamService = teamService;
        _data = data;
    }

    public int GetCurrentTeamTurn() => _currentTeamTurn + 1;

    public void EndCurrentTurn()
    {
        _currentTeamTurn++;

        if (_currentTeamTurn >= _teamService.NumberOfTeams)
        {
            _currentTeamTurn %= _teamService.NumberOfTeams;
            _data.CurrentTurnNumber++;
        }
    }

    public bool IsEndTurnProcessing { get; private set; }
    public int CurrentTurnNumber => _data.CurrentTurnNumber;
    public bool IsPlayerTurn => _teamService.IsTeamPlayable(GetCurrentTeamTurn());
}
