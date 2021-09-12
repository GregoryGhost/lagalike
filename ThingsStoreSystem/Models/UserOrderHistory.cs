namespace ThingsStore.Services
{
    using System.Collections.Generic;

    /// <summary>
    ///     An user order history.
    /// </summary>
    public record UserOrderHistory
    {
        /// <summary>
        /// Bought items. Maximum count of bought items is the last 5 bought items.
        /// </summary>
        public IList<BoughtItem> BoughtItems { get; init; }

        public uint AvailableMoney { get; init; }
    }
}