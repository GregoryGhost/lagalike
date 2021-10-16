namespace ThingsStore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using EnumWithValues;

    public record BoughtOrder
    {
        public Product Product { get; init; } = null!;
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

        public OrderInformation OrderInfo { get; init; } = null!;

        public float OrderPrice => OrderInfo.Amount * OrderInfo.Product.Cost;
    }

    public record OrderInformation
    {
        public uint Amount { get; init; }

        public Product Product { get; init; } = null!;
    }

    public record BuyerInformation
    {
        public string BuyerId { get; init; } = null!;
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

        public string Id { get; init; } = string.Empty;

        public string Label { get; init; } = string.Empty;
    }

    public class Bank : ConcurrentDictionary<string, Customer>, IBank

    {
        public Bank(IDictionary<string, Customer> customers)
            : base(customers)
        {
        }
    }

    public interface IBank : IDictionary<string, Customer>
    {
    }
}