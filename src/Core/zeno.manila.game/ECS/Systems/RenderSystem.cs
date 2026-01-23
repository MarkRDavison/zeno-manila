namespace zeno.manila.game.ECS.Systems;

public sealed class RenderSystem : WorldSystem
{
    private readonly ISpriteSheetManager _spriteSheetManager;

    public RenderSystem(ISpriteSheetManager spriteSheetManager)
    {
        _spriteSheetManager = spriteSheetManager;
    }

    public override void Update(World world, float delta)
    {
        // TODO: Layers
        // var query2 = new QueryDescription().WithAll<GroundLayer>();

        foreach (var e in world.GetWithAll<SpriteGraphicComponent, TransformComponent>())
        {
            var graphic = e.Get<SpriteGraphicComponent>();
            var kinematic = e.Get<TransformComponent>();

            Vector2? prevSize = null;

            foreach (var g in graphic.Parts)
            {
                var (info, texture) = _spriteSheetManager.GetItem(g.Sheet, g.Name);

                var offset = new Vector2();

                if (prevSize is not null)
                {
                    if (g.Offset == SpriteOffset.Column)
                    {
                        offset.Y = prevSize.Value.Y;
                    }
                }

                Raylib.DrawTexturePro(
                    texture,
                    info.Bounds,
                    new Rectangle(kinematic.Position - offset, info.Bounds.Size),
                    new Vector2(0, 0),
                    0.0f, graphic.Color);

                prevSize = info.Bounds.Size;
            }
        }
    }
}
