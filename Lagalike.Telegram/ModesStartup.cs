namespace Lagalike.Telegram
{
    using Lagalike.Telegram.Modes;

    using Microsoft.Extensions.DependencyInjection;

    public static class ModesStartup
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<DialogSystem>();
        }
    }
}