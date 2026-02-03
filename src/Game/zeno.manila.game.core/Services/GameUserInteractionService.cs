namespace zeno.manila.game.core.Services;

internal class GameUserInteractionService : IGameUserInteractionService
{
    private readonly ManilaGameCamera _manilaGameCamera;
    private readonly ManilaGameData _gameData;
    private readonly IBuildingService _buildingService;
    private readonly ITurnService _turnService;
    private readonly ISidePanelService _sidePanelService;

    public GameUserInteractionService(
        ManilaGameCamera manilaGameCamera,
        ManilaGameData gameData,
        IBuildingService buildingService,
        ITurnService turnService,
        ISidePanelService sidePanelService)
    {
        _manilaGameCamera = manilaGameCamera;
        _gameData = gameData;
        _buildingService = buildingService;
        _turnService = turnService;
        _sidePanelService = sidePanelService;
    }

    public Vector2? GetTileCoordsAtCursor()
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
        if (ActiveTile is not null && Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            ActiveTile = null;
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            ActiveTile = null;

            if (GetTileCoordsAtCursor() is { } tileCoords)
            {
                ActiveTile = tileCoords;

                var displayRelatedEntityPanel = false;

                var (city, sprawl) = _gameData.GetCityAndSprawlAtTile((int)tileCoords.X, (int)tileCoords.Y);

                if (city is not null)
                {
                    if (sprawl is not null && sprawl.RelatedEntity is { } entity)
                    {
                        if (_sidePanelService.GetActivePanel() is ManilaConstants.Panel_RelatedEntity or null)
                        {
                            _sidePanelService.DisplayPanel(ManilaConstants.Panel_RelatedEntity, entity.Name ?? string.Empty);
                            if (_sidePanelService.ActiveSidePanel is RelatedEntitySidePanel resp)
                            {
                                resp.SetRelatedEntity(entity);
                                displayRelatedEntityPanel = true;
                            }
                            // TODO: MERGE THESE?
                            if (_sidePanelService.ActiveSidePanel is IRelatedEntitySidePanel respi)
                            {
                                respi.SetRelatedEntity(entity);
                                displayRelatedEntityPanel = true;
                            }
                        }
                    }

                }

                if (!displayRelatedEntityPanel && _sidePanelService.GetActivePanel() is ManilaConstants.Panel_RelatedEntity)
                {
                    _sidePanelService.ClearPanel();
                }

                return true;
            }
        }

        return false;
    }

    public void Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            _sidePanelService.ClearPanel();
        }
        TrySelectAtCurrentMousePosition();
        HandleBuildingStuff();
    }

    private void HandleBuildingStuff()
    {
        if (_buildingService.IsBuildingModeActive && Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            DeactivateBuildingMode();
            return;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.B))
        {
            if (!_buildingService.IsBuildingModeActive)
            {
                ActivateBuildingMode();
            }
        }

        if (_buildingService.IsBuildingModeActive)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && GetTileCoordsAtCursor() is { } tileCoords)
            {
                var activeTeam = _turnService.GetCurrentTeamTurn();
                var tile = _gameData.GetSafeTile((int)tileCoords.X, (int)tileCoords.Y);
                var (city, sprawl) = _gameData.GetCityAndSprawlAtTile((int)tileCoords.X, (int)tileCoords.Y);

                if (city is not null && sprawl is not null && tile.OwningTeam == activeTeam)
                {
                    if (_buildingService.PlaceActiveBuildingAtTile((int)tileCoords.X, (int)tileCoords.Y, activeTeam))
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
        _sidePanelService.DisplayPanel(ManilaConstants.Panel_Buildings, string.Empty);
    }

    private void DeactivateBuildingMode()
    {
        _buildingService.SetBuildingModeActiveState(false);
        _sidePanelService.ClearPanel(ManilaConstants.Panel_Buildings);
    }

    public Vector2? ActiveTile { get; private set; }
}
