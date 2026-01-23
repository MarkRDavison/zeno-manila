namespace zeno.manilla.engine.Core;

public interface ISceneService
{
    void SetScene<TScene>() where TScene : IScene;

    void Init();
}
