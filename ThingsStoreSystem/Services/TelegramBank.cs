namespace Lagalike.Telegram.Modes
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using ThingsStore;

    /// <inheritdoc />
    public class TelegramBank : Bank
    {
        private static readonly IDictionary<string, Customer> Customers = new ConcurrentDictionary<string, Customer>();

        /// <inheritdoc />
        public TelegramBank()
            : base(Customers)
        {
        }
    }
}