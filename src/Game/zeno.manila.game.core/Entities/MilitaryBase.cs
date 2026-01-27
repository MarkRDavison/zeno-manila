namespace zeno.manila.game.core.Entities;

public sealed class MilitaryBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrototypeId { get; set; }
}
