namespace zeno.manila.game.core.Components;

public class BuildingsSidePanel : SidePanel
{

    private record BuildingInfo(string Name, Rectangle Bounds, bool CanAfford);

    private readonly List<string> _buildingNames;
    private readonly List<BuildingInfo> _buildings = [];
    private readonly IBuildingService _buildingService;
    private readonly ITeamService _teamService;
    private readonly ITurnService _turnService;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;

    private int _selectedIndex = -1;

    public BuildingsSidePanel(
        IBuildingService buildingService,
        ITeamService teamService,
        ITurnService turnService,
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService)
    {
        _buildingService = buildingService;
        _teamService = teamService;
        _turnService = turnService;
        _buildingPrototypeService = buildingPrototypeService;

        _buildingNames = [.. _buildingPrototypeService.GetPrototypes().Select(_ => _.Name)];
        _buildingService.OnBuildingModeChanged += (s, e) =>
        {
            _selectedIndex = -1;
        };
    }

    protected override void UpdateContent(float delta)
    {
        if (_buildings.Count > 0)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                var mouse = Raylib.GetMousePosition();
                int i = 0;
                foreach (var r in _buildings)
                {
                    if (r.CanAfford &&
                        r.Bounds.X <= mouse.X && mouse.X <= r.Bounds.X + r.Bounds.Width &&
                        r.Bounds.Y <= mouse.Y && mouse.Y <= r.Bounds.Y + r.Bounds.Height)
                    {
                        _selectedIndex = i;
                        // TODO: REPLACE WITH PASSING GUID THROUGH
                        _buildingService.SetBuildingType(_buildingNames[i].Replace(" ", ""));
                        break;
                    }

                    i++;
                }
            }
        }
    }

    protected override void DrawContent(int x, int y, int width, int height)
    {
        int xOffset = x + 8;
        int yOffset = y + 8;

        Rectangle drawAndOffset(string text, int size, Color col)
        {
            Raylib.DrawText(text, xOffset, yOffset, size, col);
            yOffset += size;

            return new Rectangle(xOffset, yOffset - size, Raylib.MeasureText(text, size), size);
        }

        drawAndOffset("Buildings", 32, Color.Black);

        yOffset += 8;

        _buildings.Clear();
        int index = 0;
        foreach (var b in _buildingNames)
        {
            var prototype = _buildingPrototypeService.GetPrototypes().ElementAt(index);
            var canAfford = _teamService.CanAfford(_turnService.GetCurrentTeamTurn(), prototype.Cost);

            var color = index == _selectedIndex
                ? Color.Yellow
                : Color.Black;

            if (canAfford is false)
            {
                color = Color.Red;
            }

            _buildings.Add(new BuildingInfo(b, drawAndOffset(b, 24, color), canAfford));
            yOffset += 4;
            index++;
        }
    }
}
