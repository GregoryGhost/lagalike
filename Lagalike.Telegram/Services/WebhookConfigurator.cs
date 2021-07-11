namespace Lagalike.Telegram.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class WebhookConfigurator : IHostedService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly TelegramBotConfiguration _botConfig;

        private readonly ILogger<WebhookConfigurator> _logger;

        private readonly string _webhookAddress;

        public WebhookConfigurator(ILogger<WebhookConfigurator> logger,
            TelegramBotConfiguration configuration,
            ConfiguredTelegramBotClient botClient)
        {
            _logger = logger;
            _botConfig = configuration;
            _botClient = botClient;
            _webhookAddress = @$"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting webhook: ", _webhookAddress);
            await _botClient.SetWebhookAsync(_webhookAddress, cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing webhook");
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}