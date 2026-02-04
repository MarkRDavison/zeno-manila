namespace zeno.manila.game.core.Entities.Military;

public sealed class MilitaryUnitPrototype : IPrototype
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, int> Cost { get; set; } = [];
    /*  TODO: AMMO?
     *  Or Just something based on how much they can fire how quickly?
     *  Need to be able to use it for training etc...
     */

}
