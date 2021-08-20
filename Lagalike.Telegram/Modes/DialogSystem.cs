namespace Lagalike.Telegram.Modes
{
    using System;
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

13        private static readonly InlineKeyboardMarkup RestartDialogInlineKeyboard = new(
            InlineKeyboardButton.WithCallbackData("Restart", "dialog start")
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

        public async Task ProccessDocumentAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);

            var file = await botClient.GetFileAsync(message.Document.FileId);

            await using var currentDocument = new MemoryStream();
            await botClient.DownloadFileAsync(file.FilePath, currentDocument);

            var parsedScenesGraph = _dialogLoader.ParseFile(currentDocument);

            if (parsedScenesGraph.IsSuccess)
            {
                await _dialogSystemCache.SetAsync(message.Chat.Id.ToString(), ObjectToByteArray(parsedScenesGraph.Value));

                var wasSuccessProccessScenes = $"The document was successfully proccessed.\n {TitleMode}";
                await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    wasSuccessProccessScenes,
                    replyMarkup: SceneWasUploadedInlineKeyboard);
            }
            else
            {
                Debug.WriteLine($"The document was failury proccessed. Error: {parsedScenesGraph.Error}");
                await botClient.SendTextMessageAsync(message.Chat.Id, "The document was failury proccessed.");
            }
        }

        public async Task ProccessSceneDialog(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var sourceScenes = await _dialogSystemCache.GetAsync(callbackQuery.From.Id.ToString());
            var scenes = FromByteArray<Graph>(sourceScenes);

            var foundDescription = GetSceneDescription(scenes, callbackQuery.Data);
            if (foundDescription.HasNoValue)
            {
                await botClient.SendTextMessageAsync(callbackQuery.From.Id, "No found the scene.");
                return;
            }

            var currentSceneDescription = foundDescription.Value;

            var foundChoices = scenes.Edges.Where(x => x.Source == currentSceneDescription);
            if (foundChoices.Any())
            {
                var choices = foundChoices.Select(
                    choice => InlineKeyboardButton.WithCallbackData(choice.Text, FormatSceneId(choice.Target.Id)));
                var choicesKeyboard = new InlineKeyboardMarkup(choices);

                await botClient.EditMessageTextAsync(
                    callbackQuery.From.Id,
                    callbackQuery.Message.MessageId,
                    currentSceneDescription.Text,
                    replyMarkup: choicesKeyboard);
            }
            else
            {
                await botClient.EditMessageTextAsync(
                    callbackQuery.From.Id,
                    callbackQuery.Message.MessageId,
                    currentSceneDescription.Text,
                    replyMarkup: RestartDialogInlineKeyboard);
            }
        }

        public async Task<Message> SendMenuAsync(ITelegramBotClient botClient, Message message)
        {
            return await SendMenuButtonsAsync(botClient, message);
        }

        private static string FormatSceneId(string targetId)
        {
            return $"dialog next scene id{targetId}";
        }

        private static T FromByteArray<T>(byte[] data)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream(data);
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }

        private static Maybe<CustomVertex> GetSceneDescription(Graph scenes, string callbackQueryData)
        {
            if (callbackQueryData == "dialog start")
                return scenes.Vertices.TryFirst();

            if (callbackQueryData.Contains("dialog next scene"))
            {
                var indexId = callbackQueryData.IndexOf("id") + 2;
                var idToFind = callbackQueryData[indexId..];

                return scenes.Vertices.TryFirst(x => x.Id == idToFind);
            }

            throw new InvalidOperationException(
                $"Cannot match {nameof(callbackQueryData)}({callbackQueryData}) with existed cases.");
        }

        private static byte[] ObjectToByteArray<T>(T obj)
        {
            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj!);

            return ms.ToArray();
        }

        private async Task<Message> SendMenuButtonsAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var wasStartedDialog = await _dialogSystemCache.GetAsync(message.From.Id.ToString()).ConfigureAwait(false) is
                { } bytes;
            if (wasStartedDialog)
                return await botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    TitleMode,
                    replyMarkup: SceneWasUploadedInlineKeyboard);

            return await botClient.SendTextMessageAsync(
                message.Chat.Id,
                TitleMode,
                replyMarkup: EmptyFileInlineKeyboard);
        }
    }
}