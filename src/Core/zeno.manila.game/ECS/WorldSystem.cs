namespace zeno.manila.game.ECS;

public abstract class WorldSystem
{
    public abstract void Update(World world, float delta);
    public virtual void UpdateNoCamera(World world, float delta) { }
}
