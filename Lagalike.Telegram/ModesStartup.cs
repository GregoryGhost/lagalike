namespace Lagalike.Telegram
{
    using Lagalike.Demo.DialogSystem.Services;
    using Lagalike.Demo.TestPatrickStar.MVU.Services;
    using Lagalike.Demo.TestPatrickStar.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    using ThingsStore.Services;

    public static class ModesStartup
    {
        public static IServiceCollection AddDemoModules(this IServiceCollection services)
        {
            services.AddModule<BackedDialogSystemModule>()
                    .AddModule<BackedThingsStoreSystemModule>()
                    .AddModule<BackedTestPatrickStarSystemModule>();

            return services;
        }

        /// <summary>
        ///     Add a demo system module.
        /// </summary>
        /// <param name="services">The host service collection.</param>
        /// <typeparam name="TBackedModeSystem">Kekw</typeparam>
        /// <returns>Updated service collection.</returns>
        public static IServiceCollection AddModule<TBackedModeSystem>(this IServiceCollection services)
            where TBackedModeSystem : IBackedModeSystem, new()
        {
            var backedMode = new TBackedModeSystem();
            backedMode.Startup.ConfigureServices(services);

            return services;
        }
    }
}