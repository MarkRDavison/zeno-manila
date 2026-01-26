namespace zeno.manilla.engine.Core;

public class Application
{
    private readonly IServiceProvider _serviceProvider;

    public static Application Instance { get; private set; } = default!;

    public Application(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        Instance = this;
    }

    public async Task Init(string title)
    {
        //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_TOPMOST);
        //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_MAXIMIZED);
        //Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_UNDECORATED);
        //var monitor = Raylib.GetCurrentMonitor();
        //var width = Raylib.GetMonitorWidth(monitor);
        //var height = Raylib.GetMonitorHeight(monitor);

        //Raylib.SetWindowSize(width, height);
        Raylib.SetExitKey(0);
        Raylib.SetExitKey(KeyboardKey.Null);
        Raylib.SetConfigFlags(ConfigFlags.VSyncHint);
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(1440, 900, title);
        Raylib.InitAudioDevice();
        Raylib.SetWindowIcon(Raylib.LoadImage("Assets/Textures/icon.png"));

        await Task.CompletedTask;
    }

    public void Stop()
    {
        Raylib.CloseWindow();
    }

    public Task Start(CancellationToken token)
    {
        while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
        {
            if (token.IsCancellationRequested)
            {
                break;
            }

            CurrentScene?.Update(1.0f / 60.0f);// Raylib.GetFrameTime());

            CurrentScene?.Draw();
        }

        return Task.CompletedTask;
    }

    public void SetScene(IScene? scene)
    {
        CurrentScene = scene;
    }

    public IScene? CurrentScene { get; private set; }

}
