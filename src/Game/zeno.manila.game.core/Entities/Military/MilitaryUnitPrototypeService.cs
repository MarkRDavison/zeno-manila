namespace zeno.manila.game.core.Entities.Military;

public sealed class MilitaryUnitPrototypeService : PrototypeService<MilitaryUnitPrototype, MilitaryUnit>
{
    public override MilitaryUnit CreateEntity(MilitaryUnitPrototype prototype)
    {
        return new MilitaryUnit
        {
            PrototypeId = prototype.Id
        };
    }
}
