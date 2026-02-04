namespace zeno.manila.game.Components;

public sealed class ButtonComponent
{
    private string _text;
    private const int _height = 24;
    private const int _padding = 8;
    private const int _border = 4;
    private int _width;

    private int _x;
    private int _y;

    private bool _withinBounds;

    private readonly IInputManager _inputManager;

    public ButtonComponent(string text, IInputManager inputManager)
    {
        SetText(text);
        _text = text;
        _inputManager = inputManager;
    }

    public void SetText(string text)
    {
        _text = text;
        _width = Raylib.MeasureText(_text, _height);
    }

    public void SetPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public void Update()
    {
        var pos = _inputManager.GetMousePosition();

        _withinBounds = false;

        if (_x <= pos.X && pos.X <= _x + TotalWidth &&
            _y <= pos.Y && pos.Y <= _y + TotalHeight)
        {
            _withinBounds = true;
        }
    }

    private int TotalWidth => _width + 2 * _padding + 2 * _border;
    private int TotalHeight => _height + 2 * _padding + 2 * _border;

    public bool WithinBounds => _withinBounds;
    public void Draw()
    {
        int xPos = _x;
        int yPos = _y;

        Raylib.DrawRectangle(
            xPos,
            yPos,
            _border * 2 + _padding * 2 + _width,
            _border * 2 + _padding * 2 + _height,
            Color.DarkGray);

        xPos += _border;
        yPos += _border;

        Raylib.DrawRectangle(
            xPos,
            yPos,
            _padding * 2 + _width,
            _padding * 2 + _height,
            Color.LightGray);

        xPos += _padding;
        yPos += _padding;

        var col = _withinBounds ? Color.Yellow : Color.Black;

        Raylib.DrawText(_text, xPos, yPos, _height, col);
    }
}
