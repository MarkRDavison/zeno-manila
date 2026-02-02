namespace zeno.manila.game.core.Entities.Buildings;

public sealed class BuildingPrototypeService : PrototypeService<BuildingPrototype, Building>
{
    public override Building CreateEntity(BuildingPrototype prototype)
    {
        return new Building
        {
            PrototypeId = prototype.Id,
            Name = prototype.Name
        };
    }
}
