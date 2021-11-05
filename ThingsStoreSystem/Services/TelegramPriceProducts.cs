namespace Lagalike.Telegram.Modes
{
    using System.Linq;

    using ThingsStore;

    /// <inheritdoc />
    public record TelegramPriceProducts : PriceProducts
    {
        /// <inheritdoc />
        public TelegramPriceProducts()
        {
            var icons = new[] { "🍟", "🍔", "🌭", "🥙", "🌮", "🌯", "🥗" };
            Products = icons.Select(
                                (icon, i) => new Product
                                {
                                    Cost = 100,
                                    Description = "Test product description",
                                    Id = i,
                                    Label = $"{icon} Test product"
                                })
                            .ToList();
        }
    }
}