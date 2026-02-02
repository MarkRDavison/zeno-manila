namespace zeno.manila.game.core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddManilaCore(this IServiceCollection services)
    {
        services
            .AddScoped<IGameUserInteractionService, GameUserInteractionService>()
            .AddScoped<ITeamService, TeamService>()
            .AddScoped<ITurnService, TurnService>()
            .AddScoped<ICityPopulationService, CityPopulationService>()
            .AddScoped<IBuildingService, BuildingService>()
            .AddScoped<ISidePanelService, SidePanelService>();

        services
            .AddScoped<IPrototypeService<BuildingPrototype, Building>, BuildingPrototypeService>();

        services
            .AddScoped<BuildingsSidePanel>()
            .AddScoped<RelatedEntitySidePanel>();

        services
            .AddScoped<ManilaGameCamera>()
            .AddScoped<ManilaGame>()
            .AddScoped<ManilaGameRenderer>()
            .AddScoped<ManilaGameData>();

        services
            .RegisterScene<ManilaGameScene>();

        return services;
    }
}
