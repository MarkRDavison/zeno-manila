namespace zeno.manilla.engine.Utility;

public static class StringHash
{
    private readonly static MD5 _md5;
    private readonly static Dictionary<Guid, string> _hashReverse = [];

    static StringHash()
    {
        _md5 = MD5.Create();
    }

    [DebuggerStepThrough]
    public static Guid Hash(string input)
    {

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = _md5.ComputeHash(inputBytes);

        var hash = new Guid(hashBytes);

        if (!_hashReverse.ContainsKey(hash))
        {
            _hashReverse.Add(hash, input);
        }

        return hash;
    }

    [DebuggerStepThrough]
    public static string ReverseHash(Guid hash)
    {
        if (_hashReverse.TryGetValue(hash, out var input))
        {
            return input;
        }

        return "NEVER HASHED THIS";
    }

}
