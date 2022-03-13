namespace ThingsStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CSharpFunctionalExtensions;

    using EnumWithValues;

    public record BoughtOrder
    {
        public IReadOnlyCollection<Product> Products { get; init; } = Array.Empty<Product>();
    }

    [EnumWithValues("ParseErrors", true, true)]
    public enum RefusalToSell
    {
        /// <summary>
        ///     Insufficient funds on the user balance.
        /// </summary>
        [EnumValue("You need more money to buy the product.")]
        InsufficientFunds,

        /// <summary>
        ///     An buyer information not found.
        /// </summary>
        [EnumValue("An buyer information not found.")]
        NotFoundBuyer,
    }

    public record Order
    {
        public BuyerInformation Buyer { get; init; } = null!;

        public IReadOnlyCollection<OrderInformation> OrderInfos { get; init; } = Array.Empty<OrderInformation>();

        public float OrderPrice => OrderInfos.Sum(x => x.Amount * x.Product.Cost);
    }

    public record OrderInfo
    {
        public float OrderPrice => OrderProducts.Values.Sum(x => x.Amount * x.Product.Cost);

        /// <summary>
        ///     Key - product id, value -
        /// </summary>
        public IDictionary<long, ProductInfo> OrderProducts { get; init; } = null!;
    }

    public record ProductInfo : OrderInformation
    {
    }

    public record OrderInformation
    {
        public uint Amount { get; init; }

        public Product Product { get; init; } = null!;
    }

    public record BuyerInformation
    {
        public long BuyerId { get; init; }
    }

    public record Customer
    {
        public float Balance { get; init; }
    }

    public record PriceProducts
    {
        public IReadOnlyCollection<Product> Products { get; init; } = Array.Empty<Product>();
    }

    public record Product
    {
        public uint Cost { get; init; }

        public string Description { get; init; } = string.Empty;

        public long Id { get; init; }

        public string Label { get; init; } = string.Empty;
    }

    /// <summary>
    ///     An bank contract - key (Customer Id), value (Customer info).
    /// </summary>
    public interface IBank : IDictionary<long, Customer>
    {
    }

    public record BasketOrderHistories
    {
        public IDictionary<long, BasketOrder> Orders { get; init; } = null!;
    }

    public record BasketOrder
    {
        public IReadOnlyCollection<OrderInformation> Items { get; init; } = Array.Empty<OrderInformation>();
    }

    public record OrderOverView
    {
        public long BoughtItemsAmount { get; init; }
        
        public long OrderId { get; init; }

        public float SpentMoney { get; init; }
    }

    /// <summary>
    ///     An basket contract - key (Customer Id), value (Orders info).
    /// </summary>
    public interface IBasket : IDictionary<long, BasketOrderHistories>
    {
        Maybe<BasketOrder> GetOrder(long customerId, long orderId);
    }

    /// <summary>
    ///     An basket contract - key (Customer Id), value (Order info).
    /// </summary>
    public interface ITempBasket : IDictionary<long, OrderInfo>
    {
        void AddProductToOrder(long customerId, Product product);

        Maybe<OrderInfo> GetOrderProducts(long customerId);

        void RemoveProductFromOrder(long customerId, long productId);
    }
}