namespace Lagalike.Telegram.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    // @formatter:off — disable formatter after this line
    using global::Telegram.Bot;
    using global::Telegram.Bot.Extensions.Polling;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    // @formatter:on — enable formatter after this line

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