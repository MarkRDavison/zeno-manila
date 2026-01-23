namespace zeno.manilla.engine.Resources;

public interface IFontManager : IDisposable
{
    void LoadFont(string name, string path);
    Font GetFont(string name);
}