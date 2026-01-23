namespace zeno.manila.game.core.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddManilaCore(this IServiceCollection services)
    {
        services
            .AddTransient<RenderSystem>();

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
