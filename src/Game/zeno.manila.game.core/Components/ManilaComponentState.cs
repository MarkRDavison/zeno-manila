namespace zeno.manila.game.core.Components;

internal sealed class ManilaComponentState : ComponentState
{
    public ManilaComponentState(
        IServiceProvider services,
        IInputManager inputManager
    ) : base(
        services,
        inputManager)
    {
    }

    public override string Click_Start => ManilaConstants.Action_Click_Start;
    public override string Click_Active => ManilaConstants.Action_Pan;
    public override string Click_End => ManilaConstants.Action_Click;
}
