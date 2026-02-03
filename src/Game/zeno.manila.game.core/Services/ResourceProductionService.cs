namespace zeno.manila.game.core.Services;

internal sealed class ResourceProductionService : IResourceProductionService
{
    private readonly ManilaGameData _data;
    private readonly IPrototypeService<BuildingPrototype, Building> _buildingPrototypeService;
    private readonly ITurnService _turnService;
    private readonly ITeamService _teamService;

    public ResourceProductionService(
        ManilaGameData data, 
        IPrototypeService<BuildingPrototype, Building> buildingPrototypeService, 
        ITurnService turnService, 
        ITeamService teamService)
    {
        _data = data;
        _buildingPrototypeService = buildingPrototypeService;
        _turnService = turnService;
        _teamService = teamService;
    }

    public void UpdateEndRound()
    {
        var teamNumber = _turnService.GetCurrentTeamTurn();

        foreach (var b in _data.Buildings)
        {
            var prototype = _buildingPrototypeService.GetPrototype(b.PrototypeId);

            foreach (var (n, a) in prototype.Production)
            {
                // TODO: Floating text
                _teamService.SetResourceAmount(teamNumber, n, a + _teamService.GetResourceAmount(teamNumber, n));
            }
        }
    }
}
