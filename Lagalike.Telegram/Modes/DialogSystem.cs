namespace Lagalike.Telegram.Modes
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    using CSharpFunctionalExtensions;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.GraphML.Parser;

    using Microsoft.Extensions.Caching.Distributed;

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
                InlineKeyboardButton.WithCallbackData("Start", "dialog start"),
                AboutModeButton
            }
        );

        private static readonly string TitleMode = $"Dialog system. {UPLOAD_TOOLTIP}";

        private readonly Loader _dialogLoader;

        private readonly DialogSystemCache _dialogSystemCache;

        public DialogSystem(Loader dialogLoader, DialogSystemCache dialogSystemCache)
        {
            _dialogLoader = dialogLoader;
            _dialogSystemCache = dialogSystemCache;
        }

        public async Task GoToNextScene(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
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

            var parsedScenesGraph = _dialogLoader.ParseFile(currentDocument);

            if (parsedScenesGraph.IsSuccess)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "The document was successfully proccessed.");
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    TitleMode,
                    replyMarkup: SceneWasUploadedInlineKeyboard);
                await _dialogSystemCache.SetAsync(message.Chat.Id.ToString(), ObjectToByteArray(parsedScenesGraph.Value));
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

        public async Task StartDialog(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var sourceScenes = await _dialogSystemCache.GetAsync(callbackQuery.From.Id.ToString());
            var scenes = FromByteArray<Graph>(sourceScenes);
            var foundCurrentDescription = scenes.Vertices.TryFirst();
            if (foundCurrentDescription.HasNoValue)
            {
                await botClient.SendTextMessageAsync(callbackQuery.From.Id, "No found the first scene.");
                return;
            }

            var currentSceneDescription = foundCurrentDescription.Value;

            var foundChoices = scenes.Edges.Where(x => x.Source == currentSceneDescription);
            if (!foundChoices.Any())
            {
                await botClient.SendTextMessageAsync(callbackQuery.From.Id, "No found choices for the first scene.");
                return;
            }

            var choices = foundChoices.Select(choice => InlineKeyboardButton.WithCallbackData(choice.Text, choice.Target.Id));
            var choicesKeyboard = new InlineKeyboardMarkup(choices);

            await botClient.SendTextMessageAsync(
                callbackQuery.From.Id,
                currentSceneDescription.Text,
                replyMarkup: choicesKeyboard);
        }

        private static T FromByteArray<T>(byte[] data)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }

        private static byte[] ObjectToByteArray<T>(T obj)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj!);

            return ms.ToArray();
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