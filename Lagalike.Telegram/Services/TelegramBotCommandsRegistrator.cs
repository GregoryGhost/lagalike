namespace Lagalike.Telegram.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Lagalike.Telegram.Shared.Services;

    using Microsoft.Extensions.Hosting;

    public class TelegramBotCommandsRegistrator : IHostedService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly DemosManager _demosManager;

        public TelegramBotCommandsRegistrator(ConfiguredTelegramBotClient botClient, DemosManager demosManager)
        {
            _botClient = botClient;
            _demosManager = demosManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await RegistrateBotCommandsAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task RegistrateBotCommandsAsync()
        {
            var botCommands = _demosManager.GetAvailableBotCommands();
            await _botClient.SetMyCommandsAsync(botCommands);
        }
    }
}