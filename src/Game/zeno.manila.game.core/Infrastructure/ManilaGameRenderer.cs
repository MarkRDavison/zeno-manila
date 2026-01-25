namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGameRenderer
{
    public void Update(float delta)
    {

    }

    public void Draw(Camera2D camera)
    {
        const int TileSize = 64;

        const int ChunkSize = 16;

        const int WorldChunkWidth = 12;
        const int WorldChunkHeight = 4;

        const int WorldTileWidth = ChunkSize * WorldChunkWidth;
        const int WorldTileHeight = ChunkSize * WorldChunkHeight;

        var position = camera.Offset;

        Raylib.BeginMode2D(camera);

        for (int y = 0; y < WorldTileHeight; ++y)
        {
            for (int x = 0; x < WorldTileWidth; ++x)
            {
                Raylib.DrawRectangle(
                    x * TileSize,
                    y * TileSize,
                    TileSize,
                    TileSize,
                    (x + y) % 2 == 0
                        ? Color.Green
                        : Color.Orange);
            }
        }

        Raylib.EndMode2D();
    }
}
