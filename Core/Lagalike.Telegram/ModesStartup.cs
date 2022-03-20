namespace Lagalike.Telegram
{
    using Lagalike.Demo.CockSizer.MVU.Services.ModuleSettings;
    using Lagalike.Demo.DialogSystem.Services;
    using Lagalike.Demo.TestPatrickStar.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    using ThingsStore.Services;

    /// <summary>
    /// Startup of all bot modes (demos).
    /// </summary>
    public static class ModesStartup
    {
        /// <summary>
        /// Add demo modules to DI.
        /// </summary>
        /// <param name="services">DI.</param>
        /// <returns>Returns DI.</returns>
        public static IServiceCollection AddDemoModules(this IServiceCollection services)
        {
            services.AddModule<BackedDialogSystemModule>()
                    .AddModule<BackedThingsStoreSystemModule>()
                    .AddModule<BackedTestPatrickStarSystemModule>()
                    .AddModule<BackedCockSizerSystemModule>();

            return services;
        }

        /// <summary>
        ///     Add a demo system module.
        /// </summary>
        /// <param name="services">The host service collection.</param>
        /// <typeparam name="TBackedModeSystem">Kekw</typeparam>
        /// <returns>Updated service collection.</returns>
        private static IServiceCollection AddModule<TBackedModeSystem>(this IServiceCollection services)
            where TBackedModeSystem : IBackedModeSystem, new()
        {
            var backedMode = new TBackedModeSystem();
            backedMode.Startup.ConfigureServices(services);

            return services;
        }
    }
}