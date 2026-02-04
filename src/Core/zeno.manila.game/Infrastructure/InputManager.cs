namespace zeno.manila.game.Infrastructure;

internal sealed class InputManager : IInputManager
{
    private readonly HashSet<string> _handled = [];
    private readonly Dictionary<string, InputAction> _actions = [];

    public bool IsActionInvoked(string name)
    {
        if (_handled.Contains(name))
        {
            return false;
        }

        if (_actions.TryGetValue(name, out var action))
        {
            if (action.Type is InputActionType.MOUSE)
            {
                if (action.State.HasFlag(InputActionState.HOLD) &&
                    Raylib.IsMouseButtonDown(action.Button))
                {
                    return true;
                }

                if (action.State.HasFlag(InputActionState.PRESS) &&
                    Raylib.IsMouseButtonPressed(action.Button))
                {
                    return true;
                }

                if (action.State.HasFlag(InputActionState.RELEASE) &&
                    Raylib.IsMouseButtonReleased(action.Button))
                {
                    return true;
                }
            }
            else if (action.Type is InputActionType.KEYBOARD)
            {
                if (action.State.HasFlag(InputActionState.HOLD) &&
                    Raylib.IsKeyDown(action.Key))
                {
                    return true;
                }

                if (action.State.HasFlag(InputActionState.PRESS) &&
                    Raylib.IsKeyPressed(action.Key))
                {
                    return true;
                }

                if (action.State.HasFlag(InputActionState.RELEASE) &&
                    Raylib.IsKeyReleased(action.Key))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HandleActionIfInvoked(string name)
    {
        if (IsActionInvoked(name))
        {
            MarkActionAsHandled(name);
            return true;
        }

        return false;
    }

    public void MarkActionAsHandled(string name)
    {
        _handled.Add(name);
    }

    public void RegisterAction(InputAction action)
    {
        _actions.Add(action.Name, action);
    }

    public void Update()
    {
        _handled.Clear();
    }

    public float GetWheelDelta() => Raylib.GetMouseWheelMove();
    public Vector2 GetMousePosition() => Raylib.GetMousePosition();
    public Vector2 GetMousePosition(Camera2D camera) => Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
}
