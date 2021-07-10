namespace Telegram.Bot.Examples.WebHook.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    public class HandleUpdateService
    {
        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(ConfiguredTelegramBotClient botClient, ILogger<HandleUpdateService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await _botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        private async Task BotOnMessageReceived(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text.Split(' ').First() switch
            {
                "/inline" => SendInlineKeyboard(_botClient, message),
                "/keyboard" => SendReplyKeyboard(_botClient, message),
                "/remove" => RemoveKeyboard(_botClient, message),
                "/photo" => SendFile(_botClient, message),
                "/request" => RequestContactAndLocation(_botClient, message),
                _ => Usage(_botClient, message)
            };
            var sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                InlineKeyboardMarkup inlineKeyboard = new(new[]
                {
                    // first row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });
                return await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    "Choose",
                    replyMarkup: inlineKeyboard);
            }

            static async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message message)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    },
                    true
                );

                return await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    "Choose",
                    replyMarkup: replyKeyboardMarkup);
            }

            static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
            {
                return await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    "Removing keyboard",
                    replyMarkup: new ReplyKeyboardRemove());
            }

            static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
            {
                await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
                return await bot.SendPhotoAsync(
                    message.Chat.Id,
                    new InputOnlineFile(fileStream, fileName),
                    "Nice Picture");
            }

            static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
            {
                ReplyKeyboardMarkup RequestReplyKeyboard = new(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });
                return await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    "Who or Where are you?",
                    replyMarkup: RequestReplyKeyboard);
            }

            static async Task<Message> Usage(ITelegramBotClient bot, Message message)
            {
                const string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n" +
                                     "/remove   - remove custom keyboard\n" +
                                     "/photo    - send a photo\n" +
                                     "/request  - request location or contact";
                return await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    usage,
                    replyMarkup: new ReplyKeyboardRemove());
            }
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        #region Inline Mode

        private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
        {
            _logger.LogInformation($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results =
            {
                // displayed result
                new InlineQueryResultArticle(
                    "3",
                    "TgBots",
                    new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await _botClient.AnswerInlineQueryAsync(
                inlineQuery.Id,
                results,
                isPersonal: true,
                cacheTime: 0);
        }

        private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
        {
            _logger.LogInformation($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        #endregion
    }
}