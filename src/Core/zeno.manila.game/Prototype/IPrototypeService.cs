namespace zeno.manila.game.Prototype;

public interface IPrototypeService<TPrototype, TEntity>
    where TPrototype : class, IPrototype
    where TEntity : class, IEntity
{
    Guid GetPrototypeId(string prototypeName);

    void RegisterPrototype(string prototypeName, TPrototype prototype);
    void RegisterPrototype(Guid prototypeId, TPrototype prototype);

    TPrototype GetPrototype(string prototypeName);
    TPrototype GetPrototype(Guid prototypeId);

    TEntity CreateEntity(string prototypeName);
    TEntity CreateEntity(Guid prototypeId);

    bool IsPrototypeRegistered(string prototypeName);
    bool IsPrototypeRegistered(Guid prototypeId);

    ICollection<TPrototype> GetPrototypes();

    string GetNameFromId(Guid prototypeId);
}