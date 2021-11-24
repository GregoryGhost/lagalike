namespace ThingsStore.Services
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <inheritdoc />
    public class TelegramBank : Bank
    {
        private static readonly IDictionary<long, Customer> Customers = new ConcurrentDictionary<long, Customer>();

        /// <inheritdoc />
        public TelegramBank()
            : base(Customers)
        {
        }
    }
}