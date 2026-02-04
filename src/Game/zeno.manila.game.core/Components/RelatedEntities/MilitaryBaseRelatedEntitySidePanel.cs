using zeno.manila.game.Components;

namespace zeno.manila.game.core.Components.RelatedEntities;

internal sealed class MilitaryBaseRelatedEntitySidePanel : BaseRelatedEntitySidePanel
{
    private readonly List<string> _unitNames = [];
    private readonly IPrototypeService<MilitaryUnitPrototype, MilitaryUnit> _militaryUnitPrototypeService;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;
    private readonly IInputManager _inputManager;

    private readonly ButtonComponent _testButton;

    public MilitaryBaseRelatedEntitySidePanel(
        IPrototypeService<MilitaryUnitPrototype, MilitaryUnit> militaryUnitPrototypeService,
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService,
        IInputManager inputManager)
    {
        _militaryUnitPrototypeService = militaryUnitPrototypeService;
        _buildingPrototypeService = buildingPrototypeService;
        _inputManager = inputManager;

        _testButton = new ButtonComponent("Hello World!", inputManager);
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

    protected override void UpdateContent(float delta)
    {
        _testButton.Update();


        if (_inputManager.IsActionInvoked(ManilaConstants.Action_Click))
        {
            var screenWidth = Raylib.GetScreenWidth();
            var screenHeight = Raylib.GetScreenHeight();

            var panelLeft = screenWidth - PanelWidth + Margin;
            var panelTop = Margin;

            var panelHeight = screenHeight - Margin * 2;

            var mousePos = _inputManager.GetMousePosition();

            if (panelLeft <= mousePos.X && mousePos.X <= panelLeft + PanelWidth &&
                panelTop <= mousePos.Y && mousePos.Y <= panelTop + panelHeight)
            {
                // TODO: TO BASE CLASS.... and helpers for getting position/bounds of panel, maybe get pos relative to panel? etc
                _inputManager.MarkActionAsHandled(ManilaConstants.Action_Click);
                Console.WriteLine("{0}.Clicked within bounds", nameof(MilitaryBaseRelatedEntitySidePanel));

                if (_testButton.WithinBounds)
                {
                    Console.WriteLine("TEST BUTTON CLICKED!!!");
                }
            }
            else
            {
                Console.WriteLine("{0}.Clicked outside bounds", nameof(MilitaryBaseRelatedEntitySidePanel));
            }
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

        _testButton.SetPosition(xOffset, yOffset + 48);

        _testButton.Draw();
    }
}
