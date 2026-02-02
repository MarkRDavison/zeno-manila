namespace zeno.manila.game.core.Entities;

public sealed class PowerPlant : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrototypeId { get; set; }
    public string? Name { get; set; } = "Power Plant";
}
