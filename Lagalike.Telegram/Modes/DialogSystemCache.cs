namespace Lagalike.Telegram.Modes
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Caching.Distributed;

    //TODO: extract to base abstract service
    public class DialogSystemCache : IDistributedCache
    {
        private const string DEMO_NAME = "dialog";

        private readonly IDistributedCache _telegramCache;

        public DialogSystemCache(IDistributedCache telegramCache)
        {
            _telegramCache = telegramCache;
        }

        public byte[] Get(string chatId)
        {
            return _telegramCache.Get(FormatCacheKey(chatId));
        }

        public Task<byte[]> GetAsync(string chatId, CancellationToken token = new CancellationToken())
        {
            return _telegramCache.GetAsync(FormatCacheKey(chatId), token);
        }

        public void Refresh(string chatId)
        {
            _telegramCache.Refresh(FormatCacheKey(chatId));
        }

        public Task RefreshAsync(string chatId, CancellationToken token = new CancellationToken())
        {
            return _telegramCache.RemoveAsync(FormatCacheKey(chatId), token);
        }

        public void Remove(string chatId)
        {
            _telegramCache.Remove(FormatCacheKey(chatId));
        }

        public Task RemoveAsync(string chatId, CancellationToken token = new CancellationToken())
        {
            return _telegramCache.RemoveAsync(FormatCacheKey(chatId), token);
        }

        public void Set(string chatId, byte[] value, DistributedCacheEntryOptions options)
        {
            _telegramCache.Set(FormatCacheKey(chatId), value, options);
        }

        public Task SetAsync(string chatId, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = new CancellationToken())
        {
            return _telegramCache.SetAsync(FormatCacheKey(chatId), value, options, token);
        }

        private static string FormatCacheKey(string chatId)
        {
            return $"{DEMO_NAME}_{chatId}";
        }
    }
}