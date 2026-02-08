namespace zeno.manila.game.Components;

public sealed class RootPanel : BaseComponent
{
    public RootPanel(
        ComponentState componentState
    ) : base(componentState)
    {
        Margin = 0;
        Padding = 0;
    }

    public override void Update()
    {
        ComponentState.Update();

        Width = ComponentState.ScreenWidth;
        Height = ComponentState.ScreenHeight;

        base.Update();
    }
}
