namespace zeno.manila.game.core.Scenes;

public sealed class ManilaGameScene : IScene
{
    private ManilaGameCamera _camera;
    private readonly ManilaGame _game;
    private readonly ManilaGameRenderer _gameRenderer;
    private readonly IGameUserInteractionService _gameUserInteractionService;
    private readonly ITurnService _turnService;
    private JsonSerializerOptions? _options;

    public ManilaGameScene(
        ManilaGameCamera camera,
        ManilaGame game,
        ManilaGameRenderer gameRenderer,
        IGameUserInteractionService gameUserInteractionService,
        ITurnService turnService)
    {
        _camera = camera;
        _game = game;
        _gameRenderer = gameRenderer;
        _gameUserInteractionService = gameUserInteractionService;
        _turnService = turnService;
    }

    public void Init()
    {
        _camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        _camera.Target = new Vector2();

        const string Level = "StarterLevel";

        var levelPath = string.Format("Assets/Levels/{0}", Level);

        var image = Raylib.LoadImage(levelPath + ".png");

        var levelDataText = File.ReadAllText(levelPath + ".json");

        if (_options is null)
        {
            _options = new JsonSerializerOptions();
            _options.Converters.Add(new Vector2JsonConverter());
        }

        var levelData = JsonSerializer.Deserialize<LevelFileData>(levelDataText, _options) ?? throw new InvalidDataException();

        _game.Init(image, levelData);

        Raylib.UnloadImage(image);
    }

    public void Update(float delta)
    {
        _camera.Update(delta);
        _gameUserInteractionService.Update();

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
