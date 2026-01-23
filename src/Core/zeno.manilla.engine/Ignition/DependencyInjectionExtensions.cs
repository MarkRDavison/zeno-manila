namespace zeno.manilla.engine.Ignition;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEngine(this IServiceCollection services)
    {
        services.AddSingleton(_ => Application.Instance);
        services.AddScoped<ISceneService, SceneService>();
        services.AddSingleton<IFontManager, FontManager>();
        services.AddSingleton<ITextureManager, TextureManager>();
        services.AddSingleton<ISpriteSheetManager, SpriteSheetManager>();

        return services;
    }

    public static IServiceCollection RegisterScene<TScene>(this IServiceCollection services) where TScene : class, IScene
    {
        services.AddTransient<TScene>();
        return services;
    }

}