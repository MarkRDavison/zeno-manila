namespace zeno.manila.game.core.Services;

internal sealed class TurnService : ITurnService
{
    private int _currentTurn;
    private readonly ITeamService _teamService;

    public TurnService(ITeamService teamService)
    {
        _teamService = teamService;
    }

    public int GetCurrentTeamTurn() => _currentTurn + 1;

    public void EndCurrentTurn()
    {
        _currentTurn = _currentTurn + 1;

        if (_currentTurn >= _teamService.NumberOfTeams)
        {
            _currentTurn %= _teamService.NumberOfTeams;
            CurrentTurnNumber++;
        }
    }

    public bool IsEndTurnProcessing { get; private set; }
    public int CurrentTurnNumber { get; private set; }
}
