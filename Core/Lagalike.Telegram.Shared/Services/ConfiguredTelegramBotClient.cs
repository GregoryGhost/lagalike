namespace Lagalike.Telegram.Shared.Services
{
    using System.Net.Http;

    using global::Telegram.Bot;

    using Microsoft.Extensions.Options;

    /// <summary>
    /// A configured Telegram client to work with Telegram API.
    /// </summary>
    public class ConfiguredTelegramBotClient : TelegramBotClient
    {
        /// <inheritdoc />
        public ConfiguredTelegramBotClient(IOptions<TelegramBotConfiguration> configuration, HttpClient httpClient)
            : base(configuration.Value.BotToken, httpClient)
        {
        }
    }
}