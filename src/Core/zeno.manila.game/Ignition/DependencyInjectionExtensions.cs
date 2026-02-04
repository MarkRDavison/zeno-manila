namespace zeno.manila.game.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddGameCore(this IServiceCollection services)
    {
        services
            .AddScoped<IInputManager, InputManager>();

        return services;
    }
}
