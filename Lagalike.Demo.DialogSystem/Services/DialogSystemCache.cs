namespace Lagalike.Telegram.Modes
{
    using Lagalike.GraphML.Parser;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// A cache for dialog system demo mode.
    /// </summary>
    public class DialogSystemCache : BaseTelegramBotCache<Graph>
    {
        /// <inheritdoc />
        public DialogSystemCache(IMemoryCache telegramCache)
            : base(telegramCache, ModeSystem.MODE_NAME)
        {
        }
    }
}