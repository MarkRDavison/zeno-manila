namespace zeno.manila.game.core.Components;

public class RelatedEntitySidePanel : SidePanel
{
    private IEntity? _entity;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;

    public RelatedEntitySidePanel(
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService)
    {
        _buildingPrototypeService = buildingPrototypeService;
    }

    public void SetRelatedEntity(IEntity? relatedEntity)
    {
        _entity = relatedEntity;
    }

    protected override void DrawContent(int x, int y, int width, int height)
    {
        if (_entity is null)
        {
            return;
        }

        // TODO: Cache drawing method?
        int xOffset = x + 8;
        int yOffset = y + 8;

        void drawAndOffset(string text, int size)
        {
            Raylib.DrawText(text, xOffset, yOffset, size, Color.Black);
            yOffset += size;
        }

        drawAndOffset(_entity.Name ?? string.Empty, 32);

        var prototype = _buildingPrototypeService.GetPrototype(_entity.PrototypeId);

        if (prototype.ActiveResources.Count is not 0)
        {
            drawAndOffset("Provides:", 24);
            foreach (var res in prototype.ActiveResources)
            {
                drawAndOffset($" - {res.Key}: {res.Value:0}", 24);
            }
        }

        if (prototype.Production.Count is not 0)
        {
            drawAndOffset("Produces:", 24);
            foreach (var res in prototype.Production)
            {
                drawAndOffset($" - {res.Key}: {res.Value:0}", 24);
            }
        }
    }
}
