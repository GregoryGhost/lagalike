namespace ThingsStore
{
    using CSharpFunctionalExtensions;

    using global::ThingsStore.Pagination;

    /// <summary>
    ///     Things store is store of product items.
    /// </summary>
    public interface IThingsStore
    {
        /// <summary>
        ///     Buy a product.
        /// </summary>
        /// <param name="item">An product item.</param>
        /// <returns>Returns an bought item.</returns>
        Result<BoughtOrder, RefusalToSell> Buy(Order buyInfo);

        /// <summary>
        ///     Get items in the store.
        /// </summary>
        /// <param name="paging">A pagination information to get items.</param>
        /// <returns>Returns paginated items.</returns>
        Paged<Product> GetItems(Pagination.Pagination paging);
    }
}