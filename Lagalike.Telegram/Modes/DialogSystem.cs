namespace Lagalike.Telegram.Modes
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.GraphML.Parser;

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

        private readonly FileReader _graphMlFileReader;

        public DialogSystem(FileReader graphMlFileReader)
        {
            _graphMlFileReader = graphMlFileReader;
        }

        public async Task ProccessDocumentAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);

            var file = await botClient.GetFileAsync(message.Document.FileId);

            await using var currentDocument = new MemoryStream();
            await botClient.DownloadFileAsync(file.FilePath, currentDocument);

            await botClient.SendTextMessageAsync(
                message.Chat.Id,
                "The document was successfully download.\n Start to parse the document.");

            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var parsedScenesGraph = _graphMlFileReader.ReadGraphMlFile(currentDocument);

            if (parsedScenesGraph.IsSuccess)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "The document was successfully proccessed.");
            }
            else
            {
                Debug.WriteLine($"The document was failury proccessed. Error: {parsedScenesGraph.Error}");
                await botClient.SendTextMessageAsync(message.Chat.Id, "The document was failury proccessed.");
            }
        }

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