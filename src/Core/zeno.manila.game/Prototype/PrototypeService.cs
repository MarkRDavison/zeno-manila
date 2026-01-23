namespace zeno.manila.game.Prototype;

public abstract class PrototypeService<TPrototype, TEntity> : IPrototypeService<TPrototype, TEntity>
    where TPrototype : class, IPrototype
    where TEntity : class, IEntity
{
    private readonly IDictionary<Guid, TPrototype> _prototypes;

    protected PrototypeService()
    {
        _prototypes = new Dictionary<Guid, TPrototype>();
    }

    public Guid GetPrototypeId(string prototypeName) => StringHash.Hash(prototypeName);

    public void RegisterPrototype(string prototypeName, TPrototype prototype) => RegisterPrototype(StringHash.Hash(prototypeName), prototype);
    public void RegisterPrototype(Guid prototypeId, TPrototype prototype) => _prototypes.Add(prototypeId, prototype);

    public TPrototype GetPrototype(string prototypeName) => GetPrototype(StringHash.Hash(prototypeName));
    public TPrototype GetPrototype(Guid prototypeId) => _prototypes[prototypeId];

    public TEntity CreateEntity(string prototypeName) => CreateEntity(StringHash.Hash(prototypeName));
    public TEntity CreateEntity(Guid prototypeId) => CreateEntity(GetPrototype(prototypeId));
    public abstract TEntity CreateEntity(TPrototype prototype);

    public bool IsPrototypeRegistered(string prototypeName) => IsPrototypeRegistered(StringHash.Hash(prototypeName));
    public bool IsPrototypeRegistered(Guid prototypeId) => _prototypes.ContainsKey(prototypeId);

    public ICollection<TPrototype> GetPrototypes() => _prototypes.Values;
    public string GetNameFromId(Guid prototypeId)
    {
        if (_prototypes.TryGetValue(prototypeId, out var prototype))
        {
            return prototype.Name;
        }

        return $"UNKNOWN PROTOTYPE ID: {prototypeId}";
    }
}
