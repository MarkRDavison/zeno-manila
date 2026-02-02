namespace zeno.manila.game.core.Components;

public class RelatedEntitySidePanel : SidePanel
{
    private IEntity? _entity;

    public void SetRelatedEntity(IEntity? entity)
    {
        _entity = entity;
    }

    protected override void DrawContent(int x, int y, int width, int height)
    {
        if (_entity is null)
        {
            return;
        }

        int xOffset = x + 8;
        int yOffset = y + 8;

        void drawAndOffset(string text, int size)
        {
            Raylib.DrawText(text, xOffset, yOffset, size, Color.Black);
            yOffset += size;
        }

        drawAndOffset(_entity.Name ?? string.Empty, 32);
    }
}
