namespace Lagalike.Telegram.Modes
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}