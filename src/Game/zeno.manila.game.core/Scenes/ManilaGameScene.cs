namespace zeno.manila.game.core.Scenes;

public sealed class ManilaGameScene : IScene
{
    private ManilaGameCamera _camera;
    private readonly ManilaGame _game;
    private readonly ManilaGameRenderer _gameRenderer;
    private readonly IGameUserInteractionService _gameUserInteractionService;
    private readonly ITurnService _turnService;
    private readonly IInputManager _inputManager;
    private JsonSerializerOptions? _options;

    private int _autoTurnDelay = 30;

    private RootPanel? _rootPanel;

    public ManilaGameScene(
        ManilaGameCamera camera,
        ManilaGame game,
        ManilaGameRenderer gameRenderer,
        IGameUserInteractionService gameUserInteractionService,
        ITurnService turnService,
        IInputManager inputManager,
        ComponentState componentState)
    {
        _camera = camera;
        _game = game;
        _gameRenderer = gameRenderer;
        _gameUserInteractionService = gameUserInteractionService;
        _turnService = turnService;
        _inputManager = inputManager;

        _rootPanel = new RootPanel(componentState);
        _rootPanel.Children.Add(new Panel(componentState)
        {
            Children =
            {
                new ButtonComponent(componentState)
                {
                    Margin = 4,
                    Padding = 4,
                    TextSize = 32,
                    Text = "Hello world!",
                    X = 24,
                    Y = 24
                }
            }
        });
    }

    public void Init()
    {
        _camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

        // TEMP
        _camera.Target = new Vector2(2800, 1500);
        _camera.Zoom = 0.45f;

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
        // TODO: Update the UI here to prevent bubbling events etc
        _rootPanel?.UpdateContainedBounds();
        _rootPanel?.Update();

        _gameRenderer.Update(delta);

        _camera.Update(delta);

        _gameUserInteractionService.Update();

        var startTurnNumber = _turnService.CurrentTurnNumber;

        // TODO: Move this to within game user interaction service?
        // Or something like orchestration? for different phases of a turn/round
        if (_turnService.IsPlayerTurn && Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            _turnService.EndCurrentTurn();
        }
        else if (!_turnService.IsPlayerTurn)
        {
            if (_autoTurnDelay-- < 0)
            {
                Console.WriteLine("Auto playing player {0}'s turn", _turnService.GetCurrentTeamTurn());
                _turnService.EndCurrentTurn();
                _autoTurnDelay = 30;
            }
        }

        if (startTurnNumber != _turnService.CurrentTurnNumber)
        {
            Console.WriteLine("Turn {0} is over.", startTurnNumber);
            _game.UpdateEndRound();
        }

        _game.Update(delta);
        _inputManager.Update();
    }


    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        _gameRenderer.Draw(_camera.Camera);
        if (_rootPanel is not null)
        {
            _gameRenderer.DrawUi(_rootPanel);
        }

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
