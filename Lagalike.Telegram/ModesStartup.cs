namespace Lagalike.Telegram
{
    using System.Collections.Generic;
    using System.Reflection;

    using Lagalike.GraphML.Parser;
    using Lagalike.Telegram.Modes;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.ApplicationParts;
    using Microsoft.Extensions.DependencyInjection;

    public static class ModesStartup
    {
        /// <summary>
        /// Add a demo system module.
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

        public static IServiceCollection AddDemoModules(this IServiceCollection services)
        {
            services.AddModule<BackedDialogSystemModule>();

            return services;
        }
    }
}