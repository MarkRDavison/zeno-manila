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
        foreach (var s in _systems)
        {
            s.Update(_world, delta);
        }
    }
}
