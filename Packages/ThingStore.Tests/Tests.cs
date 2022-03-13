namespace ThingStore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.CSharpFunctionalExtensions;

    using NUnit.Framework;

    using ThingsStore;
    using ThingsStore.Pagination;

    public class Tests
    {
        private static readonly Pagination DefaultPagination = new()
        {
            Limit = 5,
            Offset = 0
        };

        private readonly Paged<OrderOverView> _defaultPagedOrderOverview = new()
        {
            Items = Array.Empty<OrderOverView>(),
            Limit = DefaultPagination.Limit,
            Offset = DefaultPagination.Offset,
            Total = 0
        };

        [Test]
        public void BuyProductSuccess()
        {
            var (order, expectedBoughtOrder, store) = GetThingsStore();
            var boughtOrder = store.Buy(order);

            boughtOrder.Should().BeSuccess();
            boughtOrder.Value.Products.Should().BeEquivalentTo(expectedBoughtOrder.Products);
        }

        [Test]
        public void GetOrderHistoriesSuccess()
        {
            var (order, _, store) = GetThingsStore();
            store.Buy(order);

            var expectedOrderOverView = GetExpectedOrderOverView(order);
            var actualCustomerOrders = store.GetOrdersHistory(DefaultPagination, order.Buyer.BuyerId);

            actualCustomerOrders.Items.Should().BeEquivalentTo(expectedOrderOverView.Items);
        }

        [SetUp]
        public void Setup()
        {
        }

        private Paged<OrderOverView> GetExpectedOrderOverView(Order order)
        {
            return _defaultPagedOrderOverview with
            {
                Items = new List<OrderOverView>
                {
                    new()
                    {
                        BoughtItemsAmount = order.OrderInfos.Sum(x => x.Amount),
                        SpentMoney = order.OrderPrice
                    }
                },
                Total = 1,
            };
        }

        private static IReadOnlyCollection<Product> GetProducts()
        {
            var products = Enumerable.Range(5, 10)
                                     .Select(
                                         i => new Product
                                         {
                                             Cost = (uint)i,
                                             Description = $"test{i} description",
                                             Id = i,
                                             Label = $"Test{i} product"
                                         })
                                     .ToArray();
            return products;
        }

        private static (Order, BoughtOrder, IThingsStore) GetThingsStore()
        {
            const byte CustomerId = 123;
            var priceProducts = new PriceProducts
            {
                Products = GetProducts()
            };
            var bank = new Bank(
                new Dictionary<long, Customer>
                {
                    {
                        CustomerId, new Customer
                        {
                            Balance = 1000
                        }
                    }
                });
            var basket = new Basket(new Dictionary<long, BasketOrderHistories>());
            var tempBasket = new TempBasket();
            var store = new ThingsStore(priceProducts, bank, basket, tempBasket);
            var order = new Order
            {
                Buyer = new BuyerInformation
                {
                    BuyerId = bank.First().Key
                },
                OrderInfos = priceProducts.Products.Select(
                                              product =>
                                                  new OrderInformation
                                                  {
                                                      Amount = 1,
                                                      Product = product
                                                  })
                                          .ToList()
            };
            var expectedProducts = new BoughtOrder
            {
                Products = priceProducts.Products.ToList()
            };

            return (order, expectedProducts, store);
        }
    }
}