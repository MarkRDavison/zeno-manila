namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameCamera
{
    private bool _panning;

    public Vector2 Offset { get; set; }
    public Vector2 Target { get; set; }
    public float Rotation { get; set; } = 0.0f;
    public float Zoom { get; set; } = 1.0f;

    public Camera2D Camera => new(Offset, Target, Rotation, Zoom);

    public void Update(float delta)
    {
        var wheelDelta = Raylib.GetMouseWheelMove();
        if (wheelDelta is not 0)
        {
            var mouseScreen = Raylib.GetMousePosition();
            var mouseWorldBefore = Raylib.GetScreenToWorld2D(mouseScreen, Camera);

            float scale = 0.2f * wheelDelta;
            Zoom = (float)Math.Clamp(Math.Exp(Math.Log(Zoom) + scale), 0.0125f, 4.0f);

            Vector2 mouseWorldAfter = Raylib.GetScreenToWorld2D(mouseScreen, Camera);

            Target += mouseWorldBefore - mouseWorldAfter;
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            _panning = true;
        }
        else if (Raylib.IsMouseButtonDown(MouseButton.Right) && _panning)
        {
            var mouseDelta = Raylib.GetMouseDelta();

            if (mouseDelta.LengthSquared() > 0)
            {
                Target -= mouseDelta / Zoom;
            }
        }
        if (Raylib.IsMouseButtonReleased(MouseButton.Right))
        {
            _panning = false;
        }
    }
}
