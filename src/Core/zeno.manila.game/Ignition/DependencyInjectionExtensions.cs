namespace zeno.manila.game.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddGameCore(this IServiceCollection services)
    {
        services
            .AddScoped<IInputManager, InputManager>();

        return services;
    }
    public static IServiceCollection AddUi<TComponentState>(this IServiceCollection services)
        where TComponentState : ComponentState
    {
        services
            .AddScoped<ComponentState, TComponentState>();

        return services;
    }
}
