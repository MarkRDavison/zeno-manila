namespace zeno.manila.game.Components;

public class Panel : BaseComponent
{
    public int Border { get; } = 4;

    public Panel(ComponentState componentState) : base(componentState)
    {
    }

    public override void Update()
    {
        X = 0;
        Y = 0;
        Width = ComponentState.ScreenWidth;
        Height = ComponentState.ScreenHeight;

        base.Update();
    }

    public override void Draw()
    {
        Raylib.DrawRectangle(
            X + Margin,
            Y + Margin,
            Width - Margin * 2,
            Height - Margin * 2,
            Color.DarkGray);

        Raylib.DrawRectangle(
            X + Margin + Border,
            Y + Margin + Border,
            Width - Margin * 2 - Border * 2,
            Height - Margin * 2 - Border * 2,
            Color.LightGray);

        base.Draw();
    }
}
