namespace ThingsStore.Services
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     An user order history.
    /// </summary>
    public record UserOrderHistory
    {
        /// <summary>
        /// Bought items. Maximum count of bought items is the last 5 bought items.
        /// </summary>
        public IList<BoughtOrder> BoughtItems { get; init; } = Array.Empty<BoughtOrder>();

        /// <summary>
        /// Available money to buy some products.
        /// </summary>
        public uint Balance { get; init; }
    }
}