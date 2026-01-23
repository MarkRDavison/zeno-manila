namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameCamera
{
    public Vector2 Offset { get; set; }
    public Vector2 Target { get; set; }
    public float Rotation { get; set; } = 0.0f;
    public float Zoom { get; set; } = 1.0f;

    public Camera2D Camera => new(Offset, Target, Rotation, Zoom);
}
