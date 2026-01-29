namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameRenderer
{
    private bool _showDebug = true;
    private readonly ManilaGameData _gameData;
    private readonly IGameUserInteractionService _userInteractionService;
    private readonly ITeamService _teamService;
    private readonly ITurnService _turnService;

    public ManilaGameRenderer(
        ManilaGameData gameData,
        IGameUserInteractionService userInteractionService,
        ITeamService teamService,
        ITurnService turnService)
    {
        _gameData = gameData;
        _userInteractionService = userInteractionService;
        _teamService = teamService;
        _turnService = turnService;
    }

    public void Update(float delta)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F3))
        {
            _showDebug = !_showDebug;
        }
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

                var (city, sprawl) = _gameData.GetCityAndSprawlAtTile((int)activeTile2.X, (int)activeTile2.Y);

                if (city is not null && sprawl is not null && sprawl.RelatedEntity is { } entity)
                {
                    yInfoOffset += 32;
                    Raylib.DrawText(string.Format("Related entity: {0}", entity.Id), 10, yInfoOffset, 32, Color.Black);
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

            var credits = _teamService.GetResourceAmount(currentTeam, "Credits");

            Raylib.DrawText(string.Format("Credits : {0:0}", credits), 10, yInfoOffset, 32, Color.Black);
            yInfoOffset -= 32;
        }
    }
}
