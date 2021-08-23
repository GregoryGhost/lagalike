namespace Lagalike.Telegram.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Exceptions;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.Telegram.Modes;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    public class HandleUpdateService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly IMemoryCache _conversationCache;

        private readonly DialogSystem _dialogSystem;

        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(ConfiguredTelegramBotClient botClient, ILogger<HandleUpdateService> logger,
            DialogSystem dialogSystem, IMemoryCache conversationCache)
        {
            _botClient = botClient;
            _logger = logger;
            _dialogSystem = dialogSystem;
            _conversationCache = conversationCache;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(errorMessage);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            try
            {
                await GetHandlerAsync(update);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private Task GetHandlerAsync(Update update)
        {
            return update.Type switch
            {
                UpdateType.Message => ProccessReceivedMessage(update.Message),
                UpdateType.EditedMessage => ProccessReceivedMessage(update.Message),
                UpdateType.CallbackQuery => ProccessInlineKeyboardCallbackData(update.CallbackQuery),
                _ => UnknownUpdateHandlerAsync(update)
            };
        }

        private async Task ProccessInlineKeyboardCallbackData(CallbackQuery callbackQuery)
        {
            var msg = "It's nothing.";
            if (callbackQuery.Data == "About")
            {
                const string AboutMsg = "A demo of a dialog system based on the GrahpML format file.\n" +
                                        "The GraphML format files you can create in the https://www.yworks.com/products/yed.";
                msg = AboutMsg;
            }

            if (callbackQuery.Data.Contains("dialog"))
            {
                await _dialogSystem.ProccessSceneDialog(_botClient, callbackQuery);
                return;
            }

            await _botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                msg);
        }

        private async Task ProccessReceivedMessage(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");

            var hasLastCmdInfo = _conversationCache.TryGetValue(message.Chat.Id.ToString(), out var lastCmdInfo);
            if (hasLastCmdInfo)
            {
                var lastCmd = lastCmdInfo as string;
                if (lastCmd == "/dialog" && message.Type == MessageType.Document)
                {
                    await _dialogSystem.ProccessDocumentAsync(_botClient, message);
                    return;
                }
            }

            if (message.Type != MessageType.Text)
            {
                await Usage(_botClient, message);
                return;
            }

            var cmd = message.Text.Split(' ').First();
            _conversationCache.Set(
                message.Chat.Id.ToString(),
                cmd
            );

            var action = message.Text.Split(' ').First() switch
            {
                "/dialog" => _dialogSystem.SendMenuAsync(_botClient, message),
                _ => Usage(_botClient, message)
            };
            var sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        private static async Task<Message> Usage(ITelegramBotClient bot, Message message)
        {
            const string Usage = "Usage:\n" +
                                 "/dialog   - a demo of dialog system\n";

            return await bot.SendTextMessageAsync(
                message.Chat.Id,
                Usage,
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}