namespace zeno.manilla.engine.Resources;

public interface ISpriteSheetManager
{
    void LoadSpriteSheet(string name, string path);

    IEnumerable<string> GetItemNames(string name);

    (SpriteSheetItem, Texture2D) GetItem(string sheetName, string itemName);
}