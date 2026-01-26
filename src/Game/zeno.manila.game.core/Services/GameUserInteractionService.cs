using zeno.manila.game.core.Services.Contracts;

namespace zeno.manila.game.core.Services;

internal class GameUserInteractionService : IGameUserInteractionService
{
    private readonly ManilaGameCamera _manilaGameCamera;
    private readonly ManilaGameData _gameData;

    public GameUserInteractionService(
        ManilaGameCamera manilaGameCamera,
        ManilaGameData gameData)
    {
        _manilaGameCamera = manilaGameCamera;
        _gameData = gameData;
    }

    public bool TrySelectAtCurrentMousePosition()
    {
        if (ActiveTile is not null && Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            ActiveTile = null;
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            ActiveTile = null;

            var worldCoords = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _manilaGameCamera.Camera);

            if (worldCoords.X < 0 || worldCoords.Y < 0)
            {
                return false;
            }

            var tileCoords = new Vector2((int)worldCoords.X / 64, (int)worldCoords.Y / 64);

            if (tileCoords.X >= _gameData.WorldWidth ||
                tileCoords.Y >= _gameData.WorldHeight)
            {
                return false;
            }

            ActiveTile = tileCoords;

            return true;
        }

        return false;
    }

    public Vector2? ActiveTile { get; private set; }
}
