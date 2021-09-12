namespace ThingsStore.Services
{
    using Lagalike.Telegram.Shared.Services;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    ///     A cache for dialog system demo mode.
    /// </summary>
    public class ThingsStoreSystemCache : BaseTelegramBotCache<UserOrderHistory>
    {
        /// <inheritdoc />
        public ThingsStoreSystemCache(IMemoryCache telegramCache)
            : base(telegramCache, ThingsStoreModeInfo.MODE_NAME)
        {
        }
    }
}