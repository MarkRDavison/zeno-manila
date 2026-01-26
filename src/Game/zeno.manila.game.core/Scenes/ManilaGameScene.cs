namespace zeno.manila.game.core.Scenes;

public sealed class ManilaGameScene : IScene
{
    private ManilaGameCamera _camera;
    private readonly ManilaGame _game;
    private readonly ManilaGameRenderer _gameRenderer;
    private readonly ManilaGameData _data;
    private readonly IGameUserInteractionService _gameUserInteractionService;
    private readonly ITurnService _turnService;
    private JsonSerializerOptions? _options;

    public ManilaGameScene(
        ManilaGameCamera camera,
        ManilaGame game,
        ManilaGameRenderer gameRenderer,
        ManilaGameData data,
        IGameUserInteractionService gameUserInteractionService,
        ITurnService turnService)
    {
        _camera = camera;
        _game = game;
        _gameRenderer = gameRenderer;
        _data = data;
        _gameUserInteractionService = gameUserInteractionService;
        _turnService = turnService;
    }

    public void Init()
    {
        _camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        _camera.Target = new Vector2();

        const string Level = "Assets/Levels/StarterLevel";

        var image = Raylib.LoadImage(Level + ".png");

        var levelDataText = File.ReadAllText(Level + ".json");

        if (_options is null)
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new Vector2JsonConverter());
        }

        var levelData = JsonSerializer.Deserialize<LevelFileData>(levelDataText, _options) ?? throw new InvalidDataException();

        _data.WorldHeight = image.Height;
        _data.WorldWidth = image.Width;

        var maxY = _data.WorldHeight;
        var maxX = _data.WorldWidth;

        for (int y = 0; y < maxY; ++y)
        {
            _data.Tiles.Add([]);
            var newTiles = _data.Tiles.Last();

            for (int x = 0; x < maxX; ++x)
            {
                var imageColor = Raylib.GetImageColor(image, x, y);

                var tileType = TileType.Unset;

                if (imageColor.Equals(Color.Black))
                {
                    tileType = TileType.Edge;
                }
                else if (imageColor.Equals(new Color(0, 255, 0)))
                {
                    tileType = TileType.Land;
                }
                else if (imageColor.Equals(new Color(0, 0, 255)))
                {
                    tileType = TileType.Water;
                }

                var owningTeam = 0;

                if (levelData.StartLocations.IndexOf(new Vector2(x, y)) is { } index &&
                    index is not -1)
                {
                    owningTeam = index + 1;
                }

                newTiles.Add(new WorldTile
                {
                    X = x,
                    Y = y,
                    TileType = tileType,
                    OwningTeam = owningTeam
                });
            }
        }

        Raylib.UnloadImage(image);
    }

    public void Update(float delta)
    {
        _camera.Update(delta);
        _gameUserInteractionService.TrySelectAtCurrentMousePosition();

        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            _turnService.EndCurrentTurn();
        }

        _game.Update(delta);
        _gameRenderer.Update(delta);
    }


    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        _gameRenderer.Draw(_camera.Camera);

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
