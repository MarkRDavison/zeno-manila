namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameCamera
{
    private bool _panning;
    private bool _moved;
    private readonly IInputManager _inputManager;

    public ManilaGameCamera(IInputManager inputManager)
    {
        _inputManager = inputManager;
    }

    public Vector2 Offset { get; set; }
    public Vector2 Target { get; set; }
    public float Rotation { get; set; }
    public float Zoom { get; set; }

    public Camera2D Camera => new(Offset, Target, Rotation, Zoom);

    public void Update(float delta)
    {
        var wheelDelta = _inputManager.GetWheelDelta();
        if (wheelDelta is not 0)
        {
            var mouseWorldBefore = _inputManager.GetMousePosition(Camera);

            float scale = 0.2f * wheelDelta;
            Zoom = (float)Math.Clamp(Math.Exp(Math.Log(Zoom) + scale), 0.0125f, 4.0f);

            Vector2 mouseWorldAfter = _inputManager.GetMousePosition(Camera);

            Target += mouseWorldBefore - mouseWorldAfter;
        }

        if (_inputManager.HandleActionIfInvoked(ManilaConstants.Action_Click_Start))
        {
            _panning = true;
            _moved = false;
        }
        else if (_panning && _inputManager.IsActionInvoked(ManilaConstants.Action_Pan))
        {
            var mouseDelta = Raylib.GetMouseDelta();

            if (mouseDelta.LengthSquared() > 0)
            {
                _moved = true;
                _inputManager.MarkActionAsHandled(ManilaConstants.Action_Pan);
                Target -= mouseDelta / Zoom;
            }
        }

        if (_panning && _moved && _inputManager.HandleActionIfInvoked(ManilaConstants.Action_Click))
        {
            _panning = false;
        }
    }
}
