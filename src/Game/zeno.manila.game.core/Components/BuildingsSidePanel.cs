namespace zeno.manila.game.core.Components;

public class BuildingsSidePanel : SidePanel
{
    private readonly List<string> _buildingNames;
    private readonly List<Rectangle> _buildingBounds = [];

    private int _selectedIndex = -1;

    public BuildingsSidePanel()
    {
        _buildingNames = ["MilitaryBase", "PowerPlant", "Airport", "Port"];
    }

    protected override void UpdateContent(float delta)
    {
        if (_buildingBounds.Count > 0)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                var mouse = Raylib.GetMousePosition();
                int i = 0;
                foreach (var r in _buildingBounds)
                {
                    if (r.X <= mouse.X && mouse.X <= r.X + r.Width &&
                        r.Y <= mouse.Y && mouse.Y <= r.Y + r.Height)
                    {
                        _selectedIndex = i;
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

        _buildingBounds.Clear();
        int index = 0;
        foreach (var b in _buildingNames)
        {
            _buildingBounds.Add(drawAndOffset(b, 24, index == _selectedIndex ? Color.Yellow : Color.Black));
            yOffset += 4;
            index++;
        }
    }
}
