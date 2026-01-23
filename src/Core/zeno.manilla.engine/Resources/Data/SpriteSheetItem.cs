namespace zeno.manilla.engine.Resources.Data;

public record SpriteSheetItem(string Name, int X, int Y, int Width, int Height)
{
    public Rectangle Bounds => new(X, Y, Width, Height);
}
