namespace zeno.manila.game.ECS.Components;

public enum SpriteOffset
{
    None,
    Column
}
public record SpriteGraphicItem(string Sheet, string Name, SpriteOffset Offset = SpriteOffset.None);

public class SpriteGraphicComponent
{
    public List<SpriteGraphicItem> Parts { get; } = [];
    public Color Color { get; set; } = Color.White;
}
