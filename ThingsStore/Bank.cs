namespace ThingsStore
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <inheritdoc />
    public class Bank : ConcurrentDictionary<long, Customer>, IBank

    {
        /// <summary>
        ///     Initialize a new instance of Bank entity.
        /// </summary>
        /// <param name="customers">Information about customers.</param>
        public Bank(IDictionary<long, Customer> customers)
            : base(customers)
        {
        }
    }
}