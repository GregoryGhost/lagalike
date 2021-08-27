namespace Lagalike.Telegram.Shared.Services
{
    using System;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    ///     A basic implementation of cache for demos of a telegram bot.
    /// </summary>
    /// <typeparam name="TItem">A type of a saved item.</typeparam>
    public abstract class BaseTelegramBotCache<TItem> : IDisposable
    {
        private readonly string _demoCacheName;

        private readonly IMemoryCache _telegramCache;

        /// <summary>
        ///     Initalize dependencies.
        /// </summary>
        /// <param name="telegramCache">A memory cache for the Telegram.</param>
        /// <param name="demoCacheName">A cache name of demo mode.</param>
        protected BaseTelegramBotCache(IMemoryCache telegramCache, string demoCacheName)
        {
            _telegramCache = telegramCache;
            _demoCacheName = demoCacheName;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _telegramCache.Dispose();
        }

        /// <summary>
        ///     Remove a record by the Telegram chat id.
        /// </summary>
        /// <param name="chatId">A chat id.</param>
        public void Remove(string chatId)
        {
            _telegramCache.Remove(FormatCacheKey(chatId));
        }

        /// <summary>
        /// </summary>
        /// <param name="chatId">A chat id.</param>
        /// <param name="value">A saved value.</param>
        public void Set(string chatId, TItem value)
        {
            _telegramCache.Set(FormatCacheKey(chatId), value);
        }

        /// <summary>
        ///     Try to get a value from cache.
        /// </summary>
        /// <param name="chatId">A chat id.</param>
        /// <param name="value">A saved object value.</param>
        /// <returns>Returns "true" if a object value found else "false".</returns>
        public bool TryGetValue(string chatId, out TItem value)
        {
            return _telegramCache.TryGetValue(FormatCacheKey(chatId), out value);
        }

        private string FormatCacheKey(string chatId)
        {
            return $"{_demoCacheName}_{chatId}";
        }
    }
}