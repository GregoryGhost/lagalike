namespace Lagalike.Telegram.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Exceptions;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.InputFiles;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.Telegram.Modes;

    using Microsoft.Extensions.Logging;

    public class HandleUpdateService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly DialogSystem _dialogSystem;

        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(ConfiguredTelegramBotClient botClient, ILogger<HandleUpdateService> logger,
            DialogSystem dialogSystem)
        {
            _botClient = botClient;
            _logger = logger;
            _dialogSystem = dialogSystem;
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
            await _botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await _botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        private async Task ProccessReceivedMessage(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text.Split(' ').First() switch
            {
                "/dialog" => _dialogSystem.SendMenuAsync(_botClient, message),
                "/photo" => SendFile(_botClient, message),
                _ => Usage(_botClient, message)
            };
            var sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");
        }

        private static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            const string FilePath = @"Files/tux.png";
            await using FileStream fileStream = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = FilePath.Split(Path.DirectorySeparatorChar).Last();

            return await bot.SendPhotoAsync(
                message.Chat.Id,
                new InputOnlineFile(fileStream, fileName),
                "Nice Picture");
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