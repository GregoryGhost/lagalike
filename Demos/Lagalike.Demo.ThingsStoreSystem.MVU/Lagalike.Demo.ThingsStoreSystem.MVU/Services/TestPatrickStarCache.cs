namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services
{
    using Lagalike.Demo.ThingsStoreSystem.MVU.Models;
    using Lagalike.Demo.ThingsStoreSystem.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// A cache of demo Test Patrick Star.
    /// </summary>
    public class TestPatrickStarCache : BaseTelegramBotCache<Model>
    {
        /// <inheritdoc />
        public TestPatrickStarCache(IMemoryCache telegramCache, TestPatrickStarInfo modeInfo)
            : base(telegramCache, modeInfo.ModeInfo.Name)
        {
        }
    }
}