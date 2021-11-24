namespace Lagalike.Demo.DialogSystem.Services
{
    using Lagalike.GraphML.Parser;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Services;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    ///     A cache for dialog system demo mode.
    /// </summary>
    public class DialogSystemCache : BaseTelegramBotCache<Graph>
    {
        /// <inheritdoc />
        public DialogSystemCache(IMemoryCache telegramCache)
            : base(telegramCache, DialogModeInfo.MODE_NAME)
        {
        }
    }
}