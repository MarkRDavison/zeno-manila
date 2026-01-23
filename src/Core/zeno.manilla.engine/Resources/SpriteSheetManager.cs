namespace zeno.manilla.engine.Resources;

internal sealed class SpriteSheetManager : ISpriteSheetManager
{
    private readonly ITextureManager _textureManager;

    private readonly Dictionary<string, List<SpriteSheetItem>> _items = [];

    public SpriteSheetManager(ITextureManager textureManager)
    {
        _textureManager = textureManager;
    }

    public void LoadSpriteSheet(string name, string path)
    {
        var xml = XDocument.Load(path);

        var texturePath = xml.Root!.Attribute("imagePath")!.Value;

        _textureManager.LoadTexture(name, Path.Combine(Path.GetDirectoryName(path)!, texturePath));

        var items = new List<SpriteSheetItem>();

        foreach (var child in xml.Root!.Descendants())
        {
            var rectangle = new Rectangle
            {
                X = int.Parse(child.Attribute("x")!.Value),
                Y = int.Parse(child.Attribute("y")!.Value),
                Width = int.Parse(child.Attribute("width")!.Value),
                Height = int.Parse(child.Attribute("height")!.Value)
            };

            var childName = child.Attribute("name")!.Value;

            items.Add(new SpriteSheetItem(childName, (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height));
        }

        _items.Remove(name);
        _items.Add(name, items);

    }

    public IEnumerable<string> GetItemNames(string name)
    {
        if (_items.TryGetValue(name, out var items))
        {
            return items.Select(_ => _.Name);
        }

        return [];
    }

    public (SpriteSheetItem, Texture2D) GetItem(string sheetName, string itemName)
    {
        if (_items.TryGetValue(sheetName, out var items))
        {
            var item = items.FirstOrDefault(_ => _.Name == itemName);

            if (item is not null)
            {
                return (item, _textureManager.GetTexture(sheetName));
            }
        }

        throw new InvalidOperationException($"Cannot find sprite '{itemName}' in spritesheet '{sheetName}'");
    }
}
