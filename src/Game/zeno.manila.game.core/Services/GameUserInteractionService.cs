namespace zeno.manila.game.core.Services;

internal class GameUserInteractionService : IGameUserInteractionService
{
    private readonly ManilaGameCamera _manilaGameCamera;
    private readonly ManilaGameData _gameData;
    private readonly IBuildingService _buildingService;
    private readonly ITurnService _turnService;

    public GameUserInteractionService(
        ManilaGameCamera manilaGameCamera,
        ManilaGameData gameData,
        IBuildingService buildingService,
        ITurnService turnService)
    {
        _manilaGameCamera = manilaGameCamera;
        _gameData = gameData;
        _buildingService = buildingService;
        _turnService = turnService;
    }

    private Vector2? GetTileCoordsAtCursor()
    {
        var worldCoords = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _manilaGameCamera.Camera);

        if (worldCoords.X < 0 || worldCoords.Y < 0)
        {
            return null;
        }

        var tileCoords = new Vector2((int)worldCoords.X / 64, (int)worldCoords.Y / 64);

        if (tileCoords.X >= _gameData.WorldWidth ||
            tileCoords.Y >= _gameData.WorldHeight)
        {
            return null;
        }

        return tileCoords;
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

            if (GetTileCoordsAtCursor() is { } tileCoords)
            {
                ActiveTile = tileCoords;
                return true;
            }
        }

        return false;
    }

    public void Update()
    {
        TrySelectAtCurrentMousePosition();
        HandleBuildingStuff();
    }

    private void HandleBuildingStuff()
    {
        if (_buildingService.IsBuildingModeActive && Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            DeactivateBuildingMode();
            return;
        }

        if (!_buildingService.IsBuildingModeActive && Raylib.IsKeyPressed(KeyboardKey.B))
        {
            ActivateBuildingMode();
        }

        if (_buildingService.IsBuildingModeActive)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && GetTileCoordsAtCursor() is { } tileCoords)
            {
                var activeTeam = _turnService.GetCurrentTeamTurn();
                var tile = _gameData.Tiles[(int)tileCoords.Y][(int)tileCoords.X];
                if (tile.OwningTeam == activeTeam)
                {
                    var city = _gameData.Cities.FirstOrDefault(c => c.Sprawl.FirstOrDefault(_ => _.X == tileCoords.X && _.Y == tileCoords.Y) is not null);

                    var sprawl = city?.Sprawl.FirstOrDefault(_ => _.X == tileCoords.X && _.Y == tileCoords.Y);

                    if (city is not null && 
                        sprawl is not null && 
                        _buildingService.PlaceActiveBuildingAtTile((int)tileCoords.X, (int)tileCoords.Y))
                    {
                        DeactivateBuildingMode();
                    }
                }
            }
        }
    }

    private void ActivateBuildingMode()
    {
        _buildingService.SetBuildingModeActiveState(true);
    }

    private void DeactivateBuildingMode()
    {
        _buildingService.SetBuildingModeActiveState(false);
    }

    private void BuildActiveBuildingAt(int x, int y)
    {
        if (_buildingService.CanPlaceActiveBuildingAtTile(x, y))
        {
            _buildingService.PlaceActiveBuildingAtTile(x, y);
        }
    }

    private void SetActiveBuilding(string buildingType)
    {
        _buildingService.SetBuildingType(buildingType);
    }

    public Vector2? ActiveTile { get; private set; }
}
