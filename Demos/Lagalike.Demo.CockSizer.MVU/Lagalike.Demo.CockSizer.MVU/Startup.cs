namespace Lagalike.Demo.CockSizer.MVU
{
    using Lagalike.Demo.CockSizer.MVU.Services;
    using Lagalike.Demo.CockSizer.MVU.Services.ModuleSettings;
    using Lagalike.Demo.CockSizer.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IModeSystem, ModeSystem>()
                    .AddSingleton<CockSizerInfo>()
                    .AddSingleton<HandleUpdateService>()
                    .AddSingleton<DataFlowManager>()
                    .AddSingleton<ViewsFactory>()
                    .AddSingleton<MenuView>()
                    .AddSingleton<ViewMapper>()
                    .AddSingleton<DefaultViewMapper>()
                    .AddSingleton<CockSizerUpdater>()
                    .AddSingleton<CockSizerCache>()
                    .AddSingleton<CockSizerPostProccessor>()
                    .AddSingleton<CommandsFactory>();
        }
    }
}