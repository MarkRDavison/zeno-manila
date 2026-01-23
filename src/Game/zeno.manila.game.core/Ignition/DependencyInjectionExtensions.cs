namespace zeno.manila.game.core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddManilaCore(this IServiceCollection services)
    {
        services
            .AddTransient<RenderSystem>()
            .AddScoped<ManilaGameCamera>()
            .AddScoped<ManilaGame>()
            .AddScoped<ManilaGameRenderer>()
            .AddScoped<ManilaGameData>()
            .RegisterScene<FarmGameScene>();

        return services;
    }
}
