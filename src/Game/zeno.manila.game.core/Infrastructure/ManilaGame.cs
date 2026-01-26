namespace zeno.manila.game.core.Infrastructure;

public sealed class ManilaGame
{
    private readonly ManilaGameData _data;

    public ManilaGame(ManilaGameData data)
    {
        _data = data;
    }

    public void Init(Image image, LevelFileData levelData)
    {
        _data.WorldHeight = image.Height;
        _data.WorldWidth = image.Width;

        var maxY = _data.WorldHeight;
        var maxX = _data.WorldWidth;

        for (int y = 0; y < maxY; ++y)
        {
            _data.Tiles.Add([]);
            var newTiles = _data.Tiles.Last();

            for (int x = 0; x < maxX; ++x)
            {
                var imageColor = Raylib.GetImageColor(image, x, y);

                var tileType = TileType.Unset;

                if (imageColor.Equals(Color.Black))
                {
                    tileType = TileType.Edge;
                }
                else if (imageColor.Equals(new Color(0, 255, 0)))
                {
                    tileType = TileType.Land;
                }
                else if (imageColor.Equals(new Color(0, 0, 255)))
                {
                    tileType = TileType.Water;
                }

                var owningTeam = 0;

                if (levelData.StartLocations.IndexOf(new Vector2(x, y)) is { } index &&
                    index is not -1)
                {
                    owningTeam = index + 1;
                }

                newTiles.Add(new WorldTile
                {
                    X = x,
                    Y = y,
                    TileType = tileType,
                    OwningTeam = owningTeam
                });

                if (owningTeam is not 0)
                {
                    var entity = _data.World.Create(new CityComponent
                    {
                        X = x,
                        Y = y,
                        TeamNumber = owningTeam,
                        Population = 10_000
                    });

                    entity.Get<CityComponent>().RootCityEntityId = entity.Id;
                }
            }
        }
    }

    public void Update(float delta)
    {

    }
}
