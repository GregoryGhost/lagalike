namespace Lagalike.Telegram.Modes
{
    using Lagalike.Demo.DialogSystem.Services;
    using Lagalike.GraphML.Parser;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IModeSystem, ModeSystem>()
                    .AddSingleton<DialogModeInfo>()
                    .AddSingleton<HandleUpdateService>()
                    .AddSingleton<DialogSystemCache>()
                    .AddSingleton<FileReader>()
                    .AddSingleton<Loader>()
                    .AddSingleton<Parser>();
        }
    }
}