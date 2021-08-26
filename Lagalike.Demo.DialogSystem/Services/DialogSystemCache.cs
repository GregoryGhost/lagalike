namespace Lagalike.Telegram.Modes
{
    using Lagalike.GraphML.Parser;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// A cache for dialog system demo mode.
    /// </summary>
    public class DialogSystemCache : BaseTelegramBotCache<Graph>
    {
        private const string DEMO_CACHE_NAME = "dialog";

        /// <inheritdoc />
        public DialogSystemCache(IMemoryCache telegramCache)
            : base(telegramCache, DEMO_CACHE_NAME)
        {
        }
    }
}