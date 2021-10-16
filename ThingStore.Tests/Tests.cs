namespace ThingStore.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.CSharpFunctionalExtensions;

    using NUnit.Framework;

    using ThingsStore;
    using ThingsStore.Pagination;

    public class Tests
    {
        [Test]
        public void BuyProductSuccess()
        {
            var (order, expectedProduct, store) = GetThingsStore();
            var boughtOrder = store.Buy(order);

            boughtOrder.Should().BeSuccess();
            boughtOrder.Value.Product.Should().BeEquivalentTo(expectedProduct);
        }

        [SetUp]
        public void Setup()
        {
        }

        private static IReadOnlyCollection<Product> GetProducts()
        {
            var products = Enumerable.Range(5, 10)
                                     .Select(
                                         i => new Product
                                         {
                                             Cost = (uint)i,
                                             Description = $"test{i} description",
                                             Id = Guid.NewGuid().ToString(),
                                             Label = $"Test{i} product"
                                         }).ToArray();
            return products;
        }

        private static (Order, Product, IThingsStore) GetThingsStore()
        {
            var priceProducts = new PriceProducts
            {
                Products = GetProducts()
            };
            var paginator = new Paginator<Product>();
            var bank = new Bank(
                new Dictionary<string, Customer>
                {
                    {
                        "123", new Customer
                        {
                            Balance = 1000
                        }
                    }
                });
            var expectedProduct = priceProducts.Products.First();
            var order = new Order
            {
                Buyer = new BuyerInformation
                {
                    BuyerId = bank.First().Key
                },
                OrderInfo = new OrderInformation
                {
                    Amount = 1,
                    Product = expectedProduct
                }
            };
            var store = new ThingsStore(priceProducts, paginator, bank);

            return (order, expectedProduct, store);
        }
    }
}