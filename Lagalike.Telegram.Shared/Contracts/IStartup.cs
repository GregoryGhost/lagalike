namespace Lagalike.Telegram.Shared.Contracts
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Contract for a demo startup.
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// Configure services for a demo.
        /// </summary>
        /// <param name="services">DI.</param>
        void ConfigureServices(IServiceCollection services);
    }
}