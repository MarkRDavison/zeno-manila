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
            var inputManager = scope.ServiceProvider.GetRequiredService<IInputManager>();

            inputManager.RegisterAction(new()
            {
                Name = ManilaConstants.Action_Click_Start,
                Type = InputActionType.MOUSE,
                State = InputActionState.PRESS,
                Button = MouseButton.Left
            });

            inputManager.RegisterAction(new()
            {
                Name = ManilaConstants.Action_Pan,
                Type = InputActionType.MOUSE,
                State = InputActionState.DOWN,
                Button = MouseButton.Left
            });

            inputManager.RegisterAction(new()
            {
                Name = ManilaConstants.Action_Click,
                Type = InputActionType.MOUSE,
                State = InputActionState.RELEASE,
                Button = MouseButton.Left
            });

            inputManager.RegisterAction(new()
            {
                Name = ManilaConstants.Action_Escape,
                Type = InputActionType.KEYBOARD,
                State = InputActionState.RELEASE,
                Key = KeyboardKey.Escape
            });

            inputManager.RegisterAction(new()
            {
                Name = ManilaConstants.Action_BuildMode,
                Type = InputActionType.KEYBOARD,
                State = InputActionState.RELEASE,
                Key = KeyboardKey.B
            });
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
                    ActiveResources = { { ManilaConstants.Resource_Power, 80 } },
                    Cost = {
                        { ManilaConstants.Resource_Credits, 200 },
                        { ManilaConstants.Resource_Electronics, 20 },
                    }
                });
            buildingPrototypes.RegisterPrototype(
                "MilitaryBase",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("MilitaryBase"),
                    Name = "Military Base",
                    ValidTiles = [TileType.Land],
                    Cost = {
                        { ManilaConstants.Resource_Credits, 200 },
                        { ManilaConstants.Resource_Power, 20 },
                    }
                });
            buildingPrototypes.RegisterPrototype(
                "AirBase",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("AirBase"),
                    Name = "Air Base",
                    ValidTiles = [TileType.Land],
                    Cost = {
                        { ManilaConstants.Resource_Credits, 200 },
                        { ManilaConstants.Resource_Power, 20 },
                        { ManilaConstants.Resource_Electronics, 50 }
                    }
                });
            buildingPrototypes.RegisterPrototype(
                "ResourceExtractor",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("ResourceExtractor"),
                    Name = "Resource Extractor",
                    ValidTiles = [TileType.Land],
                    Cost = {
                        { ManilaConstants.Resource_Credits, 200 },
                        { ManilaConstants.Resource_Power, 20 },
                        { ManilaConstants.Resource_Electronics, 50 }
                    },
                    Production =
                    {
                        { ManilaConstants.Resource_Materials, 100 }
                    }
                });
            buildingPrototypes.RegisterPrototype(
                "ShipBuilder",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("ShipBuilder"),
                    Name = "Ship Builder",
                    ValidTiles = [TileType.Shore]
                });
            buildingPrototypes.RegisterPrototype(
                "Farm",
                new BuildingPrototype
                {
                    Id = StringHash.Hash("Farm"),
                    Name = "Farm",
                    ValidTiles = [TileType.Land],
                    ActiveResources = { { ManilaConstants.Resource_Food, 50 } },
                    Cost = {
                        { ManilaConstants.Resource_Credits, 200 },
                        { ManilaConstants.Resource_Materials, 40 },
                        { ManilaConstants.Resource_Power, 20 }
                    }
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