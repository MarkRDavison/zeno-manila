namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameRenderer
{
    public void Update(float delta)
    {

    }

    public void Draw(Camera2D camera)
    {
        Raylib.BeginMode2D(camera);

        const int Width = 24;
        const int Height = 16;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Raylib.DrawRectangle(
                    (x + 0) * ManilaConstants.TileSize,
                    (y + 0) * ManilaConstants.TileSize,
                    (x + 1) * ManilaConstants.TileSize,
                    (y + 2) * ManilaConstants.TileSize,
                    (x + y) % 2 == 0 ? Color.Green : Color.DarkGreen);
            }
        }

        Raylib.EndMode2D();
    }
}
