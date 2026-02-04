namespace zeno.manila.game.core.Components.RelatedEntities;

internal sealed class MilitaryBaseRelatedEntitySidePanel : BaseRelatedEntitySidePanel
{
    public override string Metadata => "Military Base";

    protected override void DrawContent(int x, int y, int width, int height)
    {
        if (RelatedEntity is null)
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

        drawAndOffset(RelatedEntity.Name ?? string.Empty, 32);
    }
}
