namespace ThingsStore
{
    using global::ThingsStore.Services;

    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class Startup : IStartup
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IModeSystem, ModeSystem>()
                    .AddSingleton<ThingsStoreModeInfo>()
                    .AddSingleton<ThingsStoreHandleUpdateService>()
                    .AddSingleton<ITempBasket, TempBasket>()
                    .AddSingleton<PriceProducts, TelegramPriceProducts>()
                    .AddSingleton<IBank, TelegramBank>()
                    .AddSingleton<IBasket, TelegramBasket>()
                    .AddSingleton<IThingsStore, ThingsStore>();
        }
    }
}