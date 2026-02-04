namespace zeno.manila.game.Infrastructure;

[Flags]
public enum InputActionState
{
    NONE = 0,
    PRESS = 1 << 0,
    HOLD = 1 << 1,
    RELEASE = 1 << 2,

    DOWN = PRESS | HOLD
}
