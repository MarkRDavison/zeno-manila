namespace zeno.manila.game.core.Entities.Military;

public sealed class MilitaryUnit : IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
}
