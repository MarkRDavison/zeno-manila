namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGame
{
    private readonly World _world;
    private readonly IList<WorldSystem> _systems = [];

    public ManilaGame()
    {
        _world = new();
    }

    public void Update(float delta)
    {
        for (int i = 0; i < _systems.Count; i++)
        {
            _systems[i].Update(_world, delta);
        }
    }
}
