namespace zeno.manila.game.core.Services.Contracts;

public interface IGameUserInteractionService
{
    bool TrySelectAtCurrentMousePosition();

    void Update();

    Vector2? ActiveTile { get; }
}
