namespace Lagalike.Telegram.Modes
{
    using System;

    using ThingsStore;

    /// <inheritdoc />
    public record TelegramPriceProducts: PriceProducts
    {
        /// <inheritdoc />
        public TelegramPriceProducts()
        {
            Products = new[]
            {
                new Product
                {
                    Cost = 100,
                    Description = "Test product description",
                    Id = Guid.NewGuid().ToString(),
                    Label = "Test product"
                }
            };
        }
    }
}