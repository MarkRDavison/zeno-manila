namespace zeno.manila.game.core.Entities.Buildings;

public sealed class Building : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrototypeId { get; set; }
    public string? Name { get; set; }
}
