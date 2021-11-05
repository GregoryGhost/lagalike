namespace ThingsStore
{
    using CSharpFunctionalExtensions;

    using global::ThingsStore.Pagination;

    /// <summary>
    ///     Things store is store of product items.
    /// </summary>
    public interface IThingsStore
    {
        void AddProductToOrder(long customerId, long productId);

        Maybe<OrderInfo> GetOrderInfo(long customerId);

        void RemoveProductFromOrder(long customerId, long productId);
        
        /// <summary>
        ///     Buy a product.
        /// </summary>
        /// <param name="item">An order.</param>
        /// <returns>Returns an bought item.</returns>
        Result<BoughtOrder, RefusalToSell> Buy(Order buyInfo);

        /// <summary>
        ///     Get items in the store.
        /// </summary>
        /// <param name="paging">A pagination information to get items.</param>
        /// <returns>Returns paginated items.</returns>
        Paged<Product> GetItems(Pagination.Pagination paging);

        /// <summary>
        ///     Get a order history of an customer.
        /// </summary>
        /// <param name="paging">A pagination information to get items.</param>
        /// <param name="customerId">A customer id.</param>
        /// <param name="orderId">An order id.</param>
        /// <returns>Returns an paginated order history.</returns>
        Paged<OrderInformation> GetOrderHistory(Pagination.Pagination paging, long customerId, long orderId);

        /// <summary>
        ///     Get a orders history of an customer.
        /// </summary>
        /// <param name="paging">A pagination information to get items.</param>
        /// <param name="customerId">A customer id.</param>
        /// <returns>Returns an paginated orders history.</returns>
        Paged<OrderOverView> GetOrdersHistory(Pagination.Pagination paging, long customerId);
    }
}