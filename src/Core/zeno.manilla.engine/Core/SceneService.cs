namespace zeno.manilla.engine.Core;

internal sealed class SceneService : ISceneService
{
    private readonly Application _application;
    private readonly IServiceProvider _serviceProvider;

    public SceneService(
        Application application,
        IServiceProvider serviceProvider)
    {
        _application = application;
        _serviceProvider = serviceProvider;
    }

    public void SetScene<TScene>() where TScene : IScene
    {
        var scene = _serviceProvider.GetRequiredService<TScene>();
        scene.Init();
        _application.SetScene(scene);
    }

    public void Init()
    {
        throw new NotImplementedException();
    }
}
