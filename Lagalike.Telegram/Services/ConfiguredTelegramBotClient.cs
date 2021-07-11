namespace Lagalike.Telegram.Services
{
    using System.Net.Http;

    using Microsoft.Extensions.Options;

    public class ConfiguredTelegramBotClient : TelegramBotClient
    {
        public ConfiguredTelegramBotClient(IOptions<TelegramBotConfiguration> configuration, HttpClient httpClient)
            : base(configuration.Value.BotToken, httpClient)
        {
        }
    }
}