namespace Lagalike.Telegram.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class WebhookConfigurator : IHostedService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly ILogger<WebhookConfigurator> _logger;

        private readonly string _webhookAddress;

        public WebhookConfigurator(ILogger<WebhookConfigurator> logger,
            TelegramWebhookConfiguration configuration,
            ConfiguredTelegramBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;
            _webhookAddress = configuration.WebhookAddress;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting webhook: {0}", _webhookAddress);
            await _botClient.SetWebhookAsync(_webhookAddress, cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //NOTE: don't remove a webhook because the Heroku puts to sleep the bot when the bot has no requests at 1 hour.
            return Task.CompletedTask;
        }
    }
}