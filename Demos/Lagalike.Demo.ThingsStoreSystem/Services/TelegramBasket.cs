namespace ThingsStore.Services
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <inheritdoc />
    public class TelegramBasket: Basket
    {
        private static readonly IDictionary<long, BasketOrderHistories> Orders =
            new ConcurrentDictionary<long, BasketOrderHistories>();
        
        /// <inheritdoc />
        public TelegramBasket()
            : base(Orders)
        {
        }
    }
}