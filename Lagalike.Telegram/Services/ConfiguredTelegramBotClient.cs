namespace Lagalike.Telegram.Services
{
    using System.Net.Http;

    using Microsoft.Extensions.Options;
    // @formatter:off — disable formatter after this line
    using global::Telegram.Bot;
    // @formatter:on — enable formatter after this line

    public class ConfiguredTelegramBotClient : TelegramBotClient
    {
        public ConfiguredTelegramBotClient(IOptions<TelegramBotConfiguration> configuration, HttpClient httpClient)
            : base(configuration.Value.BotToken, httpClient)
        {
        }
    }
}