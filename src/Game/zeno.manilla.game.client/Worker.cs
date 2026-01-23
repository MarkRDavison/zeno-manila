namespace zeno.manilla.game.client;

public class Worker<TStartScene> : BackgroundService
    where TStartScene : IScene
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public Worker(
        IHostApplicationLifetime hostApplicationLifetime,
        IServiceScopeFactory serviceScopeFactory)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        Console.WriteLine("TODO: Powered by Raylib");
        // TODO: Raylib.SetTraceLogCallback -> redirect output to ILogger

        using var scope = _serviceScopeFactory.CreateScope();

        var app = new Application(scope.ServiceProvider);

        await app.Init("Manila Farm");

        // TODO: Set working directory...

        var fontManager = scope.ServiceProvider.GetRequiredService<IFontManager>();
        fontManager.LoadFont("DEBUG", "Assets/Fonts/Kenney-Mini.ttf");
        fontManager.LoadFont("CALIBRIB", "Assets/Fonts/calibrib.ttf");

        var spriteManager = scope.ServiceProvider.GetRequiredService<ISpriteSheetManager>();

        var textureManager = scope.ServiceProvider.GetRequiredService<ITextureManager>();
        if (Directory.Exists("Assets/Textures"))
        {
            foreach (var f in Directory.GetFiles("Assets/Textures"))
            {
                var path = Path.GetFullPath(f);
                var filename = Path.GetFileNameWithoutExtension(path);
                textureManager.LoadTexture(filename, path);
            }
        }

        var data = scope.ServiceProvider.GetRequiredService<ManilaGameData>();

        Raylib_cs.Raylib.SetExitKey(Raylib_cs.KeyboardKey.Null);

        // TODO: Config
        scope.ServiceProvider
            .GetRequiredService<ISceneService>()
            .SetScene<TStartScene>();

        await app.Start(token);

        scope.Dispose();

        _hostApplicationLifetime.StopApplication();

        app.Stop();
    }
}