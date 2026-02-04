namespace zeno.manila.game.core.Components.RelatedEntities;

internal sealed class MilitaryBaseRelatedEntitySidePanel : BaseRelatedEntitySidePanel
{
    private readonly List<string> _unitNames = [];
    private readonly IPrototypeService<MilitaryUnitPrototype, MilitaryUnit> _militaryUnitPrototypeService;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;

    public MilitaryBaseRelatedEntitySidePanel(
        IPrototypeService<MilitaryUnitPrototype, MilitaryUnit> militaryUnitPrototypeService,
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService)
    {
        _militaryUnitPrototypeService = militaryUnitPrototypeService;
        _buildingPrototypeService = buildingPrototypeService;


    }

    public override string Metadata => "Military Base";

    protected override void OnRelatedEntitySet()
    {
        _unitNames.Clear();
        if (RelatedEntity is not null)
        {
            var canRecruit = _buildingPrototypeService.GetPrototype(RelatedEntity.PrototypeId).CanRecruit;
            _unitNames.AddRange([.. canRecruit]);
        }
    }

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

        drawAndOffset(Metadata, 32);

        if (_unitNames.Count > 0)
        {
            drawAndOffset("Recruitable", 32);
            foreach (var n in _unitNames)
            {
                drawAndOffset($" - {n}", 24);
            }
        }
    }
}
