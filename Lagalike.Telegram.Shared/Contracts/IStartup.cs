namespace Lagalike.Telegram.Shared.Contracts
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}