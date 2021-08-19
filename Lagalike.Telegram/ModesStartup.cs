namespace Lagalike.Telegram
{
    using Lagalike.GraphML.Parser;
    using Lagalike.Telegram.Modes;

    using Microsoft.Extensions.DependencyInjection;

    public static class ModesStartup
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<DialogSystem>();
            services.AddSingleton<DialogSystemCache>();

            services.AddSingleton<FileReader>();
            services.AddSingleton<Loader>();
            services.AddSingleton<Parser>();
        }
    }
}