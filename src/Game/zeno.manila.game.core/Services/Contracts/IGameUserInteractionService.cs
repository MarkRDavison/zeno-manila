namespace zeno.manila.game.core.Services.Contracts;

public interface IGameUserInteractionService
{
    bool TrySelectAtCurrentMousePosition();
    Vector2? GetTileCoordsAtCursor();
    void Update();

    Vector2? ActiveTile { get; }
}
