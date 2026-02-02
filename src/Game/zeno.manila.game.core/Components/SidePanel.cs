namespace zeno.manila.game.core.Components;

public class SidePanel
{
    public bool IsActive { get; set; }

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

        DrawContent();
    }

    protected virtual void DrawContent()
    {

    }
}
