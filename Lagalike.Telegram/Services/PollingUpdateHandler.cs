namespace Lagalike.Telegram.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     Wrapper default update handler for polling mode.
    /// </summary>
    public class PollingUpdateHandler : IUpdateHandler
    {
        private readonly HandleUpdateService _updateHandler;

        public PollingUpdateHandler(HandleUpdateService updateHandler)
        {
            _updateHandler = updateHandler;
            AllowedUpdates = null;
        }

        public UpdateType[]? AllowedUpdates { get; }

        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return _updateHandler.HandleErrorAsync(exception);
        }

        public Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            return _updateHandler.HandleUpdateAsync(update);
        }
    }
}