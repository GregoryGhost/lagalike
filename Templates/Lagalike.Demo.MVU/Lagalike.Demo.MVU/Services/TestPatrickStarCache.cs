namespace Lagalike.Demo.MVU.Services
{
    using Lagalike.Demo.MVU.Models;
    using Lagalike.Demo.MVU.Services.ModuleSettings;
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