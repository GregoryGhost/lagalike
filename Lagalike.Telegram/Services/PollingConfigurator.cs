namespace Lagalike.Telegram.Services
{
    using System.Threading;
    using System.Threading.Tasks;

    using global::Telegram.Bot;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class PollingConfigurator : IHostedService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly ILogger<PollingConfigurator> _logger;

        private readonly CancellationTokenSource _telegramClientCancellationToken;

        private readonly PollingUpdateHandler _updateHandler;

        public PollingConfigurator(ILogger<PollingConfigurator> logger, ConfiguredTelegramBotClient botClient,
            PollingUpdateHandler updateHandler)
        {
            _logger = logger;
            _botClient = botClient;
            _updateHandler = updateHandler;
            _telegramClientCancellationToken = new CancellationTokenSource();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RemoveWebhookAsync(cancellationToken);

            await StartReceiveTelegramMessangesAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _telegramClientCancellationToken.Cancel();
            return Task.CompletedTask;
        }

        private async Task RemoveWebhookAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing webhook");
            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }

        private async Task StartReceiveTelegramMessangesAsync(CancellationToken cancellationToken)
        {
            var me = await _botClient.GetMeAsync(cancellationToken);

            _botClient.StartReceiving(_updateHandler, _telegramClientCancellationToken.Token);

            _logger.LogInformation($"Start listening for @{me.Username}");
        }
    }
}