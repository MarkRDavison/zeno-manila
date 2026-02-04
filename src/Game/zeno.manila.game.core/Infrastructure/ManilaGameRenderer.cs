namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameRenderer
{
    private bool _showDebug;
    private readonly ManilaGameData _gameData;
    private readonly IGameUserInteractionService _userInteractionService;
    private readonly ITeamService _teamService;
    private readonly ITurnService _turnService;
    private readonly IBuildingService _buildingService;
    private readonly ISidePanelService _sidePanelService;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;

    public ManilaGameRenderer(
        ManilaGameData gameData,
        IGameUserInteractionService userInteractionService,
        ITeamService teamService,
        ITurnService turnService,
        IBuildingService buildingService,
        ISidePanelService sidePanelService,
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService)
    {
        _gameData = gameData;
        _userInteractionService = userInteractionService;
        _teamService = teamService;
        _turnService = turnService;
        _buildingService = buildingService;
        _sidePanelService = sidePanelService;
        _buildingPrototypeService = buildingPrototypeService;

        _buildingService.OnBuildingModeChanged += (s, e) =>
        {
            // TODO: Where should this be located???
            if (_buildingService.IsBuildingModeActive)
            {
                _sidePanelService.DisplayPanel(ManilaConstants.Panel_Buildings, string.Empty);
            }
        };
    }

    public void Update(float delta)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F3))
        {
            _showDebug = !_showDebug;
        }

        _sidePanelService.ActiveSidePanel?.Update(delta);
    }

    public void Draw(Camera2D camera)
    {
        const int TileSize = 64;

        Raylib.BeginMode2D(camera);

        for (int y = 0; y < _gameData.WorldHeight; ++y)
        {
            for (int x = 0; x < _gameData.WorldWidth; ++x)
            {
                var tile = _gameData.Tiles[y][x];

                if (tile.OwningTeam is 0)
                {
                    Raylib.DrawRectangle(
                        x * TileSize,
                        y * TileSize,
                        TileSize,
                        TileSize,
                        tile.Color);
                }
                else
                {
                    var teamColor = _teamService.GetTeamColor(tile.OwningTeam);

                    Raylib.DrawRectangle(
                        x * TileSize,
                        y * TileSize,
                        TileSize,
                        TileSize,
                        teamColor);
                }
            }
        }

        foreach (var city in _gameData.Cities)
        {
            Raylib.DrawCircle(city.X * TileSize + TileSize / 2, city.Y * TileSize + TileSize / 2, TileSize / 3, Color.Black);

            foreach (var sprawl in city.Sprawl)
            {
                if (sprawl.RelatedEntity is { } entity)
                {
                    // TODO: Draw something specific to the entity
                    Raylib.DrawCircle(
                        sprawl.X * TileSize + TileSize / 2,
                        sprawl.Y * TileSize + TileSize / 2,
                        TileSize / 4,
                        Color.Pink);
                }
            }
        }

        if (_userInteractionService.ActiveTile is { } activeTile)
        {
            Raylib.DrawRectangleLinesEx(
                new Rectangle(
                    new Vector2(
                        (int)(activeTile.X * TileSize),
                        (int)(activeTile.Y * TileSize)),
                    new Vector2(TileSize, TileSize)),
                2.0f / camera.Zoom,
                Color.White);
        }

        if (_buildingService.IsBuildingModeActive &&
            _buildingService.HasSelectedBuilding &&
            _userInteractionService.GetTileCoordsAtCursor() is { } tileCoords)
        {
            var canPlace = _buildingService.CanPlaceActiveBuildingAtTile((int)tileCoords.X, (int)tileCoords.Y, _turnService.GetCurrentTeamTurn());

            Raylib.DrawCircle(
                (int)tileCoords.X * TileSize + TileSize / 2,
                (int)tileCoords.Y * TileSize + TileSize / 2,
                TileSize / 3,
                canPlace ? Color.Green : Color.Red);

        }

        Raylib.EndMode2D();

        var mouse = Raylib.GetMousePosition();

        var world = Raylib.GetScreenToWorld2D(mouse, camera);

        int yInfoOffset = 32;

        if (_showDebug)
        {
            Raylib.DrawText(string.Format("Turn: {0:0}", _turnService.CurrentTurnNumber), 10, yInfoOffset, 32, Color.Black);
            yInfoOffset += 32;
            Raylib.DrawText(string.Format("Camera: {0:0},{1:0}", camera.Target.X, camera.Target.Y), 10, yInfoOffset, 32, Color.Black);
            yInfoOffset += 32;
            Raylib.DrawText(string.Format("Zoom: {0}", camera.Zoom), 10, yInfoOffset, 32, Color.Black);
            yInfoOffset += 32;
            Raylib.DrawText(string.Format("Mouse: {0:0},{1:0}", mouse.X, mouse.Y), 10, yInfoOffset, 32, Color.Black);
            yInfoOffset += 32;
            Raylib.DrawText(string.Format("World: {0:0},{1:0}", world.X, world.Y), 10, yInfoOffset, 32, Color.Black);
            yInfoOffset += 32;

            if (_userInteractionService.ActiveTile is { } activeTile2)
            {
                var tile = _gameData.Tiles[(int)activeTile2.Y][(int)activeTile2.X];

                Raylib.DrawText(string.Format("Active: {0:0},{1:0}", activeTile2.X, activeTile2.Y), 10, yInfoOffset, 32, Color.Black);
                yInfoOffset += 32;

                if (tile.OwningTeam is 0)
                {
                    Raylib.DrawText("Owner: Unowned", 10, yInfoOffset, 32, Color.Black);
                }
                else
                {
                    var owner = _teamService.GetTeamName(tile.OwningTeam);
                    Raylib.DrawText(string.Format("Owner: {0} ({1:0})", owner, tile.OwningTeam), 10, yInfoOffset, 32, Color.Black);
                }

                var cityCity = _gameData.GetCityAtTile((int)activeTile2.X, (int)activeTile2.Y);
                if (cityCity is not null)
                {
                    yInfoOffset += 32;
                    Raylib.DrawText(string.Format("City pop: {0}", cityCity.Population), 10, yInfoOffset, 32, Color.Black);
                }

                var (city, sprawl) = _gameData.GetCityAndSprawlAtTile((int)activeTile2.X, (int)activeTile2.Y);

                if (city is not null)
                {
                    yInfoOffset += 32;
                    Raylib.DrawText(string.Format("City pop: {0}", city.Population), 10, yInfoOffset, 32, Color.Black);
                    if (sprawl is not null && sprawl.RelatedEntity is { } entity)
                    {
                        yInfoOffset += 32;
                        var prototype = _buildingPrototypeService.GetPrototype(entity.PrototypeId);
                        Raylib.DrawText(string.Format("Related entity: {0}", prototype.Name), 10, yInfoOffset, 32, Color.Black);
                    }
                }

                yInfoOffset += 32;
            }

        }

        var currentTeam = _turnService.GetCurrentTeamTurn();
        var text = string.Format("{0}'s turn", _teamService.GetTeamName(currentTeam));
        var textWidth = Raylib.MeasureText(text, 64);

        var screenWidth = Raylib.GetScreenWidth();
        var screenHeight = Raylib.GetScreenHeight();

        var xPos = screenWidth / 2 - textWidth / 2;
        var yPos = screenHeight - 32 - 64;

        Raylib.DrawText(text, xPos, yPos, 64, _teamService.GetTeamColor(currentTeam));

        {   //  Resources
            yInfoOffset = screenHeight - 8 - 32;

            var renderResource = (string resource) =>
            {
                // TODO: Format using k for 1,000's, m for 1,000,000's etc
                var res = _teamService.GetResourceAmount(currentTeam, resource);
                Raylib.DrawText(string.Format("{0}: {1:0}", resource, res), 10, yInfoOffset, 32, Color.Black);
                yInfoOffset -= 32;
            };

            List<string> resources =
            [
                ManilaConstants.Resource_Credits,
                ManilaConstants.Resource_Power,
                ManilaConstants.Resource_Food,
                ManilaConstants.Resource_Materials,
                ManilaConstants.Resource_Electronics,
                ManilaConstants.Resource_Fuel,
                ManilaConstants.Resource_Explosives,
                ManilaConstants.Resource_Manpower
            ];

            resources.ForEach(renderResource);
        }

        _sidePanelService.ActiveSidePanel?.Draw();
    }
}
