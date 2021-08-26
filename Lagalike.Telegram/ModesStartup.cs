namespace Lagalike.Telegram
{
    using Lagalike.Telegram.Modes;

    using Microsoft.Extensions.DependencyInjection;

    public static class ModesStartup
    {
        public static IServiceCollection AddDemoModules(this IServiceCollection services)
        {
            services.AddModule<BackedDialogSystemModule>();

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