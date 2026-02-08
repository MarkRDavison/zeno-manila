namespace zeno.manila.game.Components;

public sealed class ButtonComponent : BaseComponent
{
    private int _textSize = 24;
    private string _text = string.Empty;

    public ButtonComponent(ComponentState componentState) : base(componentState)
    {
    }

    private void SetText(string text)
    {
        _text = text;
        Height = _textSize + Border * 2 + Padding * 2;
        Width = Raylib.MeasureText(_text, _textSize) + Border * 2 + Padding * 2;
    }

    public int TextSize
    {
        get => _textSize;
        set => _textSize = value;
    }
    public string Text
    {
        get => _text;
        set => SetText(value);
    }

    public int Border { get; set; } = 4;

    public override void Update()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Middle))
        {
            var mouse = ComponentState.InputManager.GetMousePosition();
            Console.WriteLine("====================================");
            Console.WriteLine("Mouse {0:0}, {1:0}", mouse.X, mouse.Y);
            Console.WriteLine("Pos {0:0}, {1:0}", X, Y);
            Console.WriteLine("Size {0:0}, {1:0}", Width, Height);
            if (Bounds.Contains(ComponentState.InputManager.GetMousePosition()))
            {
                Console.WriteLine("WITHIN BUTTON BOUNDS");
            }
            else
            {
                Console.WriteLine("OUTSIDE BUTTON BOUNDS");
            }
        }
    }

    protected override void Rearrange()
    {

    }

    public override void Draw()
    {
        int xPos = X + Margin;
        int yPos = Y + Margin;

        Raylib.DrawRectangle(
            xPos,
            yPos,
            Width,
            Height,
            Color.DarkGray);

        xPos += Border;
        yPos += Border;

        Raylib.DrawRectangle(
            xPos,
            yPos,
            Width - Border * 2,
            Height - Border * 2,
            Color.LightGray);

        xPos += Padding;
        yPos += Padding;

        var col = WithinBounds ? Color.Yellow : Color.Black;

        Raylib.DrawText(_text, xPos, yPos, TextSize, col);
    }
}
