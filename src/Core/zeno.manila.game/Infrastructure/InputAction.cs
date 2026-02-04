namespace zeno.manila.game.Infrastructure;

public sealed record InputAction
{
    public required string Name { get; init; }
    public required InputActionType Type { get; init; }
    public InputActionState State { get; set; } = InputActionState.NONE;
    public KeyboardKey Key { get; set; } = KeyboardKey.Null;
    public MouseButton Button { get; set; } = MouseButton.Extra;
}
