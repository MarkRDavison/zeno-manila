namespace zeno.manila.game.core.Services.Contracts;

public interface ITurnService
{
    int GetCurrentTeamTurn();
    void EndCurrentTurn();
    bool IsEndTurnProcessing { get; }
    int CurrentTurnNumber { get; }
    bool IsPlayerTurn { get; }
}
