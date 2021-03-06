namespace Lagalike.Telegram.Services
{
    using Lagalike.Telegram.Shared;

    using Microsoft.Extensions.Options;

    public class TelegramWebhookConfiguration
    {
        public TelegramWebhookConfiguration(IOptions<TelegramBotConfiguration> configuration)
        {
            WebhookAddress = $"{configuration.Value.HostAddress}/bot";
        }

        public string WebhookAddress { get; }
    }
}