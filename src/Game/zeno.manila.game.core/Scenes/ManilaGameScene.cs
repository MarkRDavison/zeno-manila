namespace zeno.manila.game.core.Scenes;

public sealed class ManilaGameScene : IScene
{
    private ManilaGameCamera _camera;
    private readonly ManilaGame _game;
    private readonly ManilaGameRenderer _gameRenderer;

    public ManilaGameScene(
        ManilaGameCamera camera,
        ManilaGame game,
        ManilaGameRenderer gameRenderer)
    {
        _camera = camera;
        _game = game;
        _gameRenderer = gameRenderer;
    }

    public void Init()
    {
        _camera.Zoom = 1.0f;
        _camera.Offset = new Vector2(
            Raylib.GetScreenWidth() / 2,
            Raylib.GetScreenHeight() / 2);
        _camera.Target = new Vector2(12 * 64, 6 * 64);
    }

    public void Update(float delta)
    {
        _camera.Update(delta);
        _game.Update(delta);
        _gameRenderer.Update(delta);
    }


    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.SkyBlue);

        _gameRenderer.Draw(_camera.Camera);

        Raylib.DrawFPS(10, 10);

        Raylib.EndDrawing();
    }
}
