namespace zeno.manila.game.core.Services.Contracts;

public interface IGameUserInteractionService
{
    bool TrySelectAtCurrentMousePosition();

    Vector2? ActiveTile { get; }
}
