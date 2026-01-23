namespace zeno.manilla.engine.Resources;

internal sealed class TextureManager : ITextureManager
{
    private readonly Dictionary<string, Texture2D> _textures;
    private bool disposedValue;

    public TextureManager()
    {
        _textures = [];
    }

    public void LoadTexture(string name, string path)
    {
        var texture = Raylib.LoadTexture(path);
        if (_textures.ContainsKey(name))
        {
            Raylib.UnloadTexture(_textures[name]);
            _textures[name] = texture;
        }
        else
        {
            _textures.Add(name, texture);
        }
    }

    public Texture2D GetTexture(string name)
    {
        if (_textures.TryGetValue(name, out var texture))
        {
            return texture;
        }

        throw new InvalidOperationException($"Cannot find texture with name '{name}'");
    }

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            try
            {
                if (disposing)
                {
                    foreach (var (_, texture) in _textures)
                    {
                        Raylib.UnloadTexture(texture);
                    }

                    _textures.Clear();
                }
            }
            finally
            {
                disposedValue = true;
            }
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
