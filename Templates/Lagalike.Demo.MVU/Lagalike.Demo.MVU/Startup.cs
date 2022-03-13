namespace Lagalike.Demo.MVU
{
    using Lagalike.Demo.MVU.Services;
    using Lagalike.Demo.MVU.Services.ModuleSettings;
    using Lagalike.Demo.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IModeSystem, ModeSystem>()
                    .AddSingleton<TestPatrickStarInfo>()
                    .AddSingleton<HandleUpdateService>()
                    .AddSingleton<DataFlowManager>()
                    .AddSingleton<ViewsFactory>()
                    .AddSingleton<MenuView>()
                    .AddSingleton<ViewMapper>()
                    .AddSingleton<DefaultViewMapper>()
                    .AddSingleton<TestPatrickUpdater>()
                    .AddSingleton<TestPatrickStarCache>()
                    .AddSingleton<TestPatrickPostProccessor>()
                    .AddSingleton<CommandsFactory>();
        }
    }
}