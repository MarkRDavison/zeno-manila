namespace zeno.manila.game.core.Components;

public class SidePanel
{
    protected const int Margin = 8;
    protected const int PanelWidth = 480;
    protected const int Border = 8;
    public bool IsActive { get; set; } = true;

    public void Update(float delta)
    {
        if (!IsActive)
        {
            return;
        }

        UpdateContent(delta);
    }

    protected virtual void UpdateContent(float delta)
    {

    }

    public void Draw()
    {
        if (!IsActive)
        {
            return;
        }

        var width = Raylib.GetScreenWidth();
        var height = Raylib.GetScreenHeight();

        Raylib.DrawRectangle(
            Margin + width - PanelWidth,
            Margin,
            PanelWidth - 2 * Margin,
            height - 2 * Margin,
            Color.DarkGray);

        Raylib.DrawRectangle(
            width - PanelWidth + Border + Margin,
            Border + Margin,
            PanelWidth - Border * 2 - 2 * Margin,
            height - Border * 2 - 2 * Margin,
            Color.LightGray);

        DrawContent(
            width - PanelWidth + Border + Margin,
            Border + Margin,
            PanelWidth - Border * 2 - 2 * Margin,
            height - Border * 2 - 2 * Margin);
    }

    protected virtual void DrawContent(int x, int y, int width, int height)
    {

    }
}
