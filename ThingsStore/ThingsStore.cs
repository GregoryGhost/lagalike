namespace ThingsStore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using CSharpFunctionalExtensions;

    using global::ThingsStore.Pagination;

    /// <inheritdoc />
    public class ThingsStore : IThingsStore
    {
        /// <summary>
        ///     Bank - an key is id buyer, an value is information about buyer.
        /// </summary>
        private readonly IBank _bank;

        private readonly IBasket _basket;

        private readonly IReadOnlyCollection<Product> _products;

        private readonly ITempBasket _tempBasket;

        public ThingsStore(PriceProducts price, IBank bank, IBasket basket, ITempBasket tempBasket)
        {
            _bank = bank;
            _basket = basket;
            _tempBasket = tempBasket;
            _products = price.Products;
        }

        /// <inheritdoc />
        public void AddProductToOrder(long customerId, long productId)
        {
            var product = _products.FirstOrDefault(x => x.Id == productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Not found the product id {productId}");
            }
            _tempBasket.AddProductToOrder(customerId, product);
        }

        /// <inheritdoc />
        public void RemoveProductFromOrder(long customerId, long productId)
        {
            var product = _products.FirstOrDefault(x => x.Id == productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Not found the product id {productId}");
            }
            _tempBasket.RemoveProductFromOrder(customerId, productId);
        }

        /// <inheritdoc />
        public Result<BoughtOrder, RefusalToSell> Buy(Order buyInfo)
        {
            var order = IsSolventBuyer(buyInfo);
            if (!order.IsSuccess)
                return order.Error;

            var boughtProduct = PayOrder(order.Value);

            return boughtProduct;
        }

        /// <inheritdoc />
        public Paged<Product> GetItems(Pagination.Pagination paging)
        {
            return _products.Paginate(paging);
        }

        /// <inheritdoc />
        public Paged<OrderInformation> GetOrderHistory(Pagination.Pagination paging, long customerId, long orderId)
        {
            return _basket.GetOrder(customerId, orderId)
                          .Match(
                              x => x.Items.Paginate(paging),
                              () => new Paged<OrderInformation>());
        }

        /// <inheritdoc />
        public Maybe<OrderInfo> GetOrderInfo(long customerId)
        {
            return _tempBasket.GetOrderProducts(customerId);
        }

        /// <inheritdoc />
        public Paged<OrderOverView> GetOrdersHistory(Pagination.Pagination paging, long customerId)
        {
            if (!_basket.TryGetValue(customerId, out var foundCustomer))
                return new Paged<OrderOverView>();

            var customerOrders = foundCustomer
                                 .Orders.Values.Select(
                                     x =>
                                     {
                                         var boughtItemsAmount = x.Items.Sum(y => y.Amount);
                                         var spentMoney = x.Items.Sum(y => y.Amount * y.Product.Cost);

                                         return new OrderOverView
                                         {
                                             BoughtItemsAmount = boughtItemsAmount,
                                             SpentMoney = spentMoney
                                         };
                                     })
                                 .Paginate(paging);

            return customerOrders;
        }

        private void AddOrderToBasketHistory(Order order)
        {
            if (!_basket.TryGetValue(order.Buyer.BuyerId, out var customerOrders))
            {
                Debug.WriteLine("Not found customer id in the basket");
                return;
            }

            var basketOrder = new BasketOrder
            {
                Items = order.OrderInfos
            };
            var basketId = customerOrders.Orders.Keys.Count + 1;
            customerOrders.Orders.Add(basketId, basketOrder);
        }

        private void AddOrderToNewBasketHistory(Order order)
        {
            var basketOrder = new BasketOrder
            {
                Items = order.OrderInfos
            };
            var customerOrdersHistory = new BasketOrderHistories
            {
                Orders = new ConcurrentDictionary<long, BasketOrder>()
            };
            customerOrdersHistory.Orders.Add(
                0,
                basketOrder);
            _basket.Add(order.Buyer.BuyerId, customerOrdersHistory);
        }

        private void ExecuterPaymentTransaction(Order order)
        {
            var customer = _bank[order.Buyer.BuyerId];
            _bank[order.Buyer.BuyerId] = customer with { Balance = customer.Balance - order.OrderPrice };
        }

        private Result<Order, RefusalToSell> IsSolventBuyer(Order order)
        {
            if (!_bank.TryGetValue(order.Buyer.BuyerId, out var foundBuyer))
                return RefusalToSell.NotFoundBuyer;

            var canBuyOrder = foundBuyer.Balance >= order.OrderPrice;

            return canBuyOrder ? order : RefusalToSell.InsufficientFunds;
        }

        private BoughtOrder PayOrder(Order order)
        {
            ExecuterPaymentTransaction(order);
            UpdateOrderHistory(order);
            _tempBasket.Remove(order.Buyer.BuyerId);
            var products = order.OrderInfos.Select(x => x.Product).ToList();

            return new BoughtOrder
            {
                Products = products,
            };
        }

        private void UpdateOrderHistory(Order order)
        {
            if (_basket.ContainsKey(order.Buyer.BuyerId))
                AddOrderToBasketHistory(order);
            else
                AddOrderToNewBasketHistory(order);
        }
    }
}