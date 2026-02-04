namespace zeno.manila.game.Infrastructure.Contracts;

public interface IInputManager
{
    public bool IsActionInvoked(string name);
    public bool HandleActionIfInvoked(string name);
    public void MarkActionAsHandled(string name);
    public void RegisterAction(InputAction action);
    public void Update();

    float GetWheelDelta();
    Vector2 GetMousePosition();
    Vector2 GetMousePosition(Camera2D camera);
}
