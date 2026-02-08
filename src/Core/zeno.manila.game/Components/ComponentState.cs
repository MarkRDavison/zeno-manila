namespace zeno.manila.game.Components;

public abstract class ComponentState
{
    public int ScreenWidth { get; private set; }
    public int ScreenHeight { get; private set; }
    public bool ViewportChanged { get; private set; }
    public IServiceProvider Services { get; init; }
    public IInputManager InputManager { get; init; }

    public abstract string Click_Start { get; }
    public abstract string Click_Active { get; }
    public abstract string Click_End { get; }

    public ComponentState(
        IServiceProvider services,
        IInputManager inputManager)
    {
        Services = services;
        InputManager = inputManager;
    }

    public void Update()
    {
        ViewportChanged = false;
        var width = Raylib.GetScreenWidth();
        var height = Raylib.GetScreenHeight();

        if (ScreenWidth != width || ScreenHeight != height)
        {
            ViewportChanged = true;
            ScreenWidth = width;
            ScreenHeight = height;
        }
    }
}
