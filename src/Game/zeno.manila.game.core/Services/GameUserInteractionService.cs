namespace zeno.manila.game.core.Services;

internal class GameUserInteractionService : IGameUserInteractionService
{
    private readonly ManilaGameCamera _manilaGameCamera;
    private readonly ManilaGameData _gameData;
    private readonly IBuildingService _buildingService;
    private readonly ITurnService _turnService;
    private readonly ISidePanelService _sidePanelService;
    private readonly IInputManager _inputManager;

    public GameUserInteractionService(
        ManilaGameCamera manilaGameCamera,
        ManilaGameData gameData,
        IBuildingService buildingService,
        ITurnService turnService,
        ISidePanelService sidePanelService,
        IInputManager inputManager)
    {
        _manilaGameCamera = manilaGameCamera;
        _gameData = gameData;
        _buildingService = buildingService;
        _turnService = turnService;
        _sidePanelService = sidePanelService;
        _inputManager = inputManager;
    }

    public Vector2? GetTileCoordsAtCursor()
    {
        var worldCoords = _inputManager.GetMousePosition(_manilaGameCamera.Camera);

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
        if (ActiveTile is not null &&
            _inputManager.HandleActionIfInvoked(ManilaConstants.Action_Escape))
        {
            ActiveTile = null;
        }

        if (_inputManager.HandleActionIfInvoked(ManilaConstants.Action_Click))
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
        HandleBuildingStuff();

        if (_sidePanelService.ActiveSidePanel is not null &&
            _inputManager.HandleActionIfInvoked(ManilaConstants.Action_Escape))
        {
            _sidePanelService.ClearPanel();
        }

        TrySelectAtCurrentMousePosition();
    }

    private void HandleBuildingStuff()
    {
        if (_buildingService.IsBuildingModeActive &&
            _inputManager.HandleActionIfInvoked(ManilaConstants.Action_Escape))
        {
            DeactivateBuildingMode();
            return;
        }

        if (!_buildingService.IsBuildingModeActive &&
            _inputManager.HandleActionIfInvoked(ManilaConstants.Action_BuildMode))
        {
            ActivateBuildingMode();
        }


        if (_buildingService.IsBuildingModeActive)
        {
            if (_inputManager.HandleActionIfInvoked(ManilaConstants.Action_Click) &&
                GetTileCoordsAtCursor() is { } tileCoords)
            {
                var activeTeam = _turnService.GetCurrentTeamTurn();
                var tile = _gameData.GetSafeTile((int)tileCoords.X, (int)tileCoords.Y);
                var (city, sprawl) = _gameData.GetCityAndSprawlAtTile((int)tileCoords.X, (int)tileCoords.Y);

                if (city is not null && sprawl is not null && tile.OwningTeam == activeTeam)
                {
                    if (_buildingService.PlaceActiveBuildingAtTile((int)tileCoords.X, (int)tileCoords.Y, activeTeam))
                    {
                        ActiveTile = tileCoords;
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
