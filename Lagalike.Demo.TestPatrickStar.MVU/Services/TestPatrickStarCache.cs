namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using Lagalike.Demo.TestPatrickStar.MVU.Models;
    using Lagalike.Demo.TestPatrickStar.MVU.Services.ModuleSettings;
    using Lagalike.Telegram.Shared.Contracts;

    using Microsoft.Extensions.Caching.Memory;

    public class TestPatrickStarCache : BaseTelegramBotCache<Model>
    {
        /// <inheritdoc />
        public TestPatrickStarCache(IMemoryCache telegramCache, TestPatrickStarInfo modeInfo)
            : base(telegramCache, modeInfo.ModeInfo.Name)
        {
        }
    }
}