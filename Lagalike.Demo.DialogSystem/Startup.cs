namespace Lagalike.Telegram.Modes
{
    using Lagalike.GraphML.Parser;

    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ModeSystem>();
            services.AddSingleton<ITelegramUpdateHandler, HandleUpdateService>();
            services.AddSingleton<DialogSystemCache>();

            services.AddSingleton<FileReader>();
            services.AddSingleton<Loader>();
            services.AddSingleton<Parser>();
        }
    }
}