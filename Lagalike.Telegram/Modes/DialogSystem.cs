namespace Lagalike.Telegram.Modes
{
    using System.Threading.Tasks;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.ReplyMarkups;

    public class DialogSystem
    {
        private const string UPLOAD_TOOLTIP = "You can upload a GraphML file at any time.";

        private static readonly InlineKeyboardButton AboutModeButton = InlineKeyboardButton.WithCallbackData("About");

        private static readonly InlineKeyboardMarkup EmptyFileInlineKeyboard = new(AboutModeButton);

        private static readonly InlineKeyboardMarkup SceneWasStartedInlineKeyboard = new(
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Resume"),
                InlineKeyboardButton.WithCallbackData("Restart"),
                AboutModeButton
            }
        );

        private static readonly InlineKeyboardMarkup SceneWasUploadedInlineKeyboard = new(
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Start"),
                AboutModeButton
            }
        );

        private static readonly string TitleMode = $"Dialog system. {UPLOAD_TOOLTIP}";

        public async Task<Message> SendMenuAsync(ITelegramBotClient botClient, Message message)
        {
            return await SendMenuButtonsAsync(botClient, message);
        }

        private static async Task<Message> SendMenuButtonsAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            // Simulate longer running task
            await Task.Delay(500);

            return await botClient.SendTextMessageAsync(
                message.Chat.Id,
                TitleMode,
                replyMarkup: EmptyFileInlineKeyboard);
        }
    }
}