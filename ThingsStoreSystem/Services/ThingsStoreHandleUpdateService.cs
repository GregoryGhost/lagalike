namespace ThingsStore.Services
{
    using System.Threading.Tasks;

    using Lagalike.Telegram.Shared;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Services;

    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using Telegram.Bot.Types.ReplyMarkups;

    using ThingsStore.Constants;

    public class ThingsStoreHandleUpdateService : ITelegramUpdateHandler
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly InlineKeyboardMarkup _menuInlineKeyboard;

        private readonly ModeInfo _modeInfo;

        private readonly ThingsStoreSystemCache _thingsStoreSystemCache;

        private readonly string _titleMode;

        public ThingsStoreHandleUpdateService(ConfiguredTelegramBotClient botClient,
            ThingsStoreSystemCache thingsStoreSystemCache,
            ThingsStoreModeInfo modeInfo)
        {
            _botClient = botClient;
            _titleMode = $"It's things store - {modeInfo.ModeInfo.ShortDescription}";
            _thingsStoreSystemCache = thingsStoreSystemCache;
            _modeInfo = modeInfo.ModeInfo;

            var goShoppingButton = GetInlineKeyboardButton(AvailableDemoCommands.GoShopping);
            var showOrderHistory = GetInlineKeyboardButton(AvailableDemoCommands.ShowOrderHistory);
            var aboutModeButton = GetInlineKeyboardButton(AvailableDemoCommands.About);
            _menuInlineKeyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[] { goShoppingButton },
                    new[] { showOrderHistory },
                    new[] { aboutModeButton }
                }
            );
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var action = update.Type switch
            {
                UpdateType.Message or UpdateType.EditedMessage => ProccessReceivedMessageAsync(update.Message),
                UpdateType.CallbackQuery => ProccessInlineKeyboardCallbackDataAsync(update.CallbackQuery),
                _ => Task.CompletedTask
            };

            await action;
        }

        private static string FormatSelectionItem(string itemId)
        {
            return $"{ThingsStoreModeInfo.MODE_NAME} select item id{itemId}";
        }

        private static InlineKeyboardButton GetInlineKeyboardButton(CommandInfo command)
        {
            var (userLabel, commandValue) = command;
            return InlineKeyboardButton.WithCallbackData(userLabel, commandValue);
        }

        private static bool IsCommandToSelectItemToBuy(string callbackQueryData)
        {
            return callbackQueryData.Contains(AvailableDemoCommands.SelectItem.CommandValue);
        }

        private async Task ProccessInlineKeyboardCallbackDataAsync(CallbackQuery callbackQuery)
        {
            
            
            if (callbackQuery.Data == AvailableDemoCommands.GoShopping.CommandValue)
            {
                await ProccessShoppingAsync(callbackQuery);
                return;
            }

            if (callbackQuery.Data == AvailableDemoCommands.SelectItem.CommandValue)
            {
                await ProccessSelectItemAsync(callbackQuery);
                return;
            }

            if (callbackQuery.Data == AvailableDemoCommands.BuyItem.CommandValue)
            {
                await ProccessBuyItemAsync(callbackQuery);
                return;
            }

            if (callbackQuery.Data == AvailableDemoCommands.ShowOrderHistory.CommandValue)
            {
                await ProccessShowOrderHistory(callbackQuery);
                return;
            }

            var msg = "It's nothing.";
            if (callbackQuery.Data == AvailableDemoCommands.About.CommandValue)
                msg = _modeInfo.AboutDescription;

            await _botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                msg);
        }

        private async Task ProccessShowOrderHistory(CallbackQuery callbackQuery)
        {
            throw new System.NotImplementedException();
        }

        private async Task ProccessBuyItemAsync(CallbackQuery callbackQuery)
        {
            throw new System.NotImplementedException();
        }

        private async Task ProccessSelectItemAsync(CallbackQuery callbackQuery)
        {
            throw new System.NotImplementedException();
        }

        private async Task ProccessShoppingAsync(CallbackQuery callbackQuery)
        {
            var itemsKeyboard = new InlineKeyboardMarkup(
                new []
                {
                    new []{ InlineKeyboardButton.WithCallbackData("item 1", FormatSelectionItem("1")) },
                    new []{ InlineKeyboardButton.WithCallbackData("item 2", FormatSelectionItem("2")) },
                    new []{ InlineKeyboardButton.WithCallbackData("item 3", FormatSelectionItem("3")) }
                });
            await _botClient.EditMessageTextAsync(
                callbackQuery.From.Id,
                callbackQuery.Message.MessageId,
                "Available items in the things store. You have 400 money.",
                replyMarkup: itemsKeyboard);
        }

        private async Task ProccessReceivedMessageAsync(Message message)
        {
            if (message.Type == MessageType.Text)
                await SendMenuButtonsAsync(message);
        }

        private async Task SendMenuButtonsAsync(Message message)
        {
            await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                _titleMode,
                replyMarkup: _menuInlineKeyboard);
        }
    }
}