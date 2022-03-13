namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using global::PatrickStar.MVU;

    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.Telegram.Shared.Services;

    using Newtonsoft.Json;

    /// <summary>
    /// Telegram update processor.
    /// </summary>
    /// <typeparam name="TCmdType">A command type, which contains in views of Patrick Star MVU.</typeparam>
    public abstract class TelegramPostProccessor<TCmdType> : IPostProccessor<TCmdType, TelegramUpdate>
        where TCmdType : Enum
    {
        private readonly ConfiguredTelegramBotClient _client;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="client">Telegram client.</param>
        protected TelegramPostProccessor(ConfiguredTelegramBotClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task ProccessAsync(IView<TCmdType> view, TelegramUpdate update)
        {
            var inlineButtons = view.Menu.Buttons.Select(
                                        buttons => buttons.Select(
                                            button =>
                                            {
                                                var serializedCommand = JsonConvert.SerializeObject(button.Cmd);
                                                return InlineKeyboardButton.WithCallbackData(
                                                    button.Label,
                                                    serializedCommand);
                                            })
                                    )
                                    .ToArray();
            var keyboard = new InlineKeyboardMarkup(inlineButtons);

            if (update.RequestType is RequestTypes.CallbackData)
                await _client.EditMessageTextAsync(
                    update.ChatId,
                    update.Update.CallbackQuery.Message.MessageId,
                    view.Menu.MessageElement.Text,
                    replyMarkup: keyboard);
            else if (update.RequestType is RequestTypes.Message or RequestTypes.EditedMessage)
                await _client.SendTextMessageAsync(update.ChatId, view.Menu.MessageElement.Text, replyMarkup: keyboard);
            else
                throw new NotImplementedException($"Cannot proccess this action {update.RequestType}");
        }
    }
}