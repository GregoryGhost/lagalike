namespace Lagalike.Telegram.Services
{
    using Microsoft.Extensions.Options;

    public class TelegramWebhookConfiguration
    {
        public TelegramWebhookConfiguration(IOptions<TelegramBotConfiguration> configuration)
        {
            WebhookAddress = configuration.Value.HostAddress;
        }

        public string WebhookAddress { get; }
    }
}