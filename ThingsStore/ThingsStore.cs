namespace ThingsStore
{
    using System.Collections.Generic;

    using CSharpFunctionalExtensions;

    using global::ThingsStore.Pagination;

    /// <inheritdoc />
    public class ThingsStore : IThingsStore
    {
        /// <summary>
        ///     Bank - an key is id buyer, an value is information about buyer.
        /// </summary>
        private readonly IBank _bank;

        private readonly Paginator<Product> _paginator;

        private readonly IReadOnlyCollection<Product> _products;

        public ThingsStore(PriceProducts price, Paginator<Product> paginator, IBank bank)
        {
            _paginator = paginator;
            _bank = bank;
            _products = price.Products;
        }

        /// <inheritdoc />
        public Result<BoughtOrder, RefusalToSell> Buy(Order buyInfo)
        {
            var order = IsSolventBuyer(buyInfo);
            if (!order.IsSuccess)
                return order.Error;

            var boughtProduct = BuyProduct(order.Value);

            return boughtProduct;
        }

        /// <inheritdoc />
        public Paged<Product> GetItems(Pagination.Pagination paging)
        {
            var pagedProducts = _paginator.Paginate(paging, _products);

            return pagedProducts;
        }

        private BoughtOrder BuyProduct(Order order)
        {
            var customer = _bank[order.Buyer.BuyerId];
            _bank[order.Buyer.BuyerId] = customer with { Balance = customer.Balance - order.OrderPrice };

            return new BoughtOrder
            {
                Product = order.OrderInfo.Product,
            };
        }

        private Result<Order, RefusalToSell> IsSolventBuyer(Order order)
        {
            if (!_bank.TryGetValue(order.Buyer.BuyerId, out var foundBuyer))
                return RefusalToSell.NotFoundBuyer;

            var canBuyOrder = foundBuyer.Balance >= order.OrderPrice;

            return canBuyOrder ? order : RefusalToSell.InsufficientFunds;
        }
    }
}