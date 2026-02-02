using zeno.manila.game.core;
using zeno.manila.game.core.Entities.Buildings;
using zeno.manila.game.Prototype;
using zeno.manilla.engine.Utility;

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

        {
            var buildingPrototypes = scope.ServiceProvider.GetRequiredService<IPrototypeService<BuildingPrototype, Building>>();

            buildingPrototypes.RegisterPrototype(
                "PowerPlant",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("PowerPlant"),
                    Name = "Power Plant",
                    ValidTiles = [TileType.Land],
                    ActiveResources = { { ManilaConstants.Resource_Power, 80 } }
                });
            buildingPrototypes.RegisterPrototype(
                "MilitaryBase",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("MilitaryBase"),
                    Name = "Military Base",
                    ValidTiles = [TileType.Land]
                });
            buildingPrototypes.RegisterPrototype(
                "ShipBuilder",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("ShipBuilder"),
                    Name = "Ship Builder",
                    ValidTiles = [TileType.Shore]
                });
        }

        var data = scope.ServiceProvider.GetRequiredService<ManilaGameData>();

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