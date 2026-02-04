namespace zeno.manilla.engine.Extensions;

public static class RectangleExtensions
{
    public static bool Contains(this Rectangle rectangle, Vector2 point)
    {
        return
            rectangle.X <= point.X && point.X <= rectangle.X + rectangle.Width &&
            rectangle.Y <= point.Y && point.Y <= rectangle.Y + rectangle.Height;
    }
}
