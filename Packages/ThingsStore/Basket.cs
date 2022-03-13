namespace ThingsStore
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using CSharpFunctionalExtensions;

    /// <inheritdoc />
    public class Basket : ConcurrentDictionary<long, BasketOrderHistories>, IBasket
    {
        /// <summary>
        ///     Initialize a new instance of Basket entity.
        /// </summary>
        /// <param name="orders">Information about orders.</param>
        public Basket(IDictionary<long, BasketOrderHistories> orders)
            : base(orders)
        {
        }

        /// <inheritdoc />
        public Maybe<BasketOrder> GetOrder(long customerId, long orderId)
        {
            if (!TryGetValue(customerId, out var foundCustomer) ||
                !foundCustomer.Orders.TryGetValue(orderId, out var orderInfo))
                return Maybe<BasketOrder>.None;

            return orderInfo;
        }
    }
}