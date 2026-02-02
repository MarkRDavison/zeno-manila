namespace zeno.manila.game.Prototype;

public interface IEntity
{
    public Guid Id { get; set; }
    public Guid PrototypeId { get; set; }
    public string? Name { get; set; }
}
