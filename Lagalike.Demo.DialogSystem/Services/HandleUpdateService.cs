namespace Lagalike.Telegram.Modes
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CSharpFunctionalExtensions;

    using global::Telegram.Bot;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.Demo.DialogSystem.Constants;
    using Lagalike.GraphML.Parser;

    public class HandleUpdateService : ITelegramUpdateHandler
    {
        private const string UPLOAD_TOOLTIP = "You can upload a GraphML file at any time.";

        private static readonly InlineKeyboardButton AboutModeButton = GetInlineKeyboardButton(AvailableDemoCommands.About);

        private static readonly InlineKeyboardMarkup EmptyFileInlineKeyboard = new(AboutModeButton);

        private static readonly InlineKeyboardMarkup RestartDialogInlineKeyboard = new(
            GetInlineKeyboardButton(AvailableDemoCommands.Restart)
        );

        private static readonly InlineKeyboardMarkup SceneWasUploadedInlineKeyboard = new(
            new[]
            {
                GetInlineKeyboardButton(AvailableDemoCommands.Start),
                AboutModeButton
            }
        );

        private static readonly string TitleMode = $"Dialog system. {UPLOAD_TOOLTIP}";

        private static readonly string WasSuccessfullyProccessedSceneDocument =
            $"The document was successfully proccessed.\n {TitleMode}";

        private readonly ITelegramBotClient _botClient;

        private readonly Loader _dialogLoader;

        private readonly DialogSystemCache _dialogSystemCache;

        private readonly ModeInfo _modeInfo;

        public HandleUpdateService(ITelegramBotClient botClient, Loader dialogLoader, DialogSystemCache dialogSystemCache,
            ModeSystem modeSystem)
        {
            _botClient = botClient;
            _dialogLoader = dialogLoader;
            _dialogSystemCache = dialogSystemCache;
            _modeInfo = modeSystem.Info;
        }

        public Task HandleUpdateAsync(Update update)
        {
            throw new NotImplementedException();
        }

        private static string FormatSceneId(string targetId)
        {
            return $"{ModeSystem.MODE_NAME} next scene id{targetId}";
        }

        private static InlineKeyboardButton GetInlineKeyboardButton(CommandInfo command)
        {
            var (userLabel, commandValue) = command;
            return InlineKeyboardButton.WithCallbackData(userLabel, commandValue);
        }

        private static Maybe<CustomVertex> GetNextScene(Graph scenes, string callbackQueryData)
        {
            var indexId = GetNextScene(callbackQueryData);
            var idToFind = callbackQueryData[indexId..];

            return scenes.Vertices.TryFirst(x => x.Id == idToFind);
        }

        private static int GetNextScene(string callbackQueryData)
        {
            var indexId = callbackQueryData.IndexOf("id") + 2;
            return indexId;
        }

        private static Maybe<CustomVertex> GetSceneDescription(Graph scenes, string callbackQueryData)
        {
            if (callbackQueryData == AvailableDemoCommands.Start.CommandValue)
                return scenes.Vertices.TryFirst();

            if (IsCommandToProccessNextSceneDialog(callbackQueryData))
                return GetNextScene(scenes, callbackQueryData);

            throw new InvalidOperationException(
                $"Cannot match {nameof(callbackQueryData)}({callbackQueryData}) with existed cases.");
        }

        private async Task<MemoryStream> GetSceneFileAsync(Message message)
        {
            var file = await _botClient.GetFileAsync(message.Document.FileId);

            await using var currentDocument = new MemoryStream();
            await _botClient.DownloadFileAsync(file.FilePath, currentDocument);

            return currentDocument;
        }

        private static bool IsCommandToProccessNextSceneDialog(string callbackQueryData)
        {
            return callbackQueryData.Contains(AvailableDemoCommands.ProccessNextScene.CommandValue);
        }

        private static bool IsCommandToProccessSceneDialog(CallbackQuery callbackQuery)
        {
            return callbackQuery.Data.Contains(ModeSystem.MODE_NAME);
        }

        private async Task ParseSceneFile(Message message, MemoryStream currentDocument)
        {
            var parsedScenesGraph = _dialogLoader.ParseFile(currentDocument);

            if (parsedScenesGraph.IsSuccess)
            {
                _dialogSystemCache.Set(message.Chat.Id.ToString(), parsedScenesGraph.Value);

                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    WasSuccessfullyProccessedSceneDocument,
                    replyMarkup: SceneWasUploadedInlineKeyboard);
            }
            else
            {
                Debug.WriteLine($"The document was failury proccessed. Error: {parsedScenesGraph.Error}");
                await _botClient.SendTextMessageAsync(message.Chat.Id, "The document was failury proccessed.");
            }
        }

        private async Task ProccessDocumentAsync(Message message)
        {
            await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadDocument);

            await using var currentDocument = await GetSceneFileAsync(message);

            await ParseSceneFile(message, currentDocument);
        }

        private async Task ProccessInlineKeyboardCallbackData(CallbackQuery callbackQuery)
        {
            var msg = "It's nothing.";
            if (callbackQuery.Data == AvailableDemoCommands.About.CommandValue)
                msg = _modeInfo.AboutDescription;

            if (IsCommandToProccessSceneDialog(callbackQuery))
            {
                await ProccessSceneDialog(callbackQuery);
                return;
            }

            await _botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                msg);
        }

        private async Task ProccessSceneDialog(CallbackQuery callbackQuery)
        {
            var hasScenes = _dialogSystemCache.TryGetValue(callbackQuery.From.Id.ToString(), out var scenes);
            if (!hasScenes)
            {
                await _botClient.SendTextMessageAsync(callbackQuery.From.Id, "Have no the scene.");
                return;
            }

            var foundDescription = GetSceneDescription(scenes, callbackQuery.Data);
            if (foundDescription.HasNoValue)
            {
                await _botClient.SendTextMessageAsync(callbackQuery.From.Id, "No found the scene.");
                return;
            }

            var currentSceneDescription = foundDescription.Value;

            var foundChoices = scenes.Edges.Where(x => x.Source == currentSceneDescription);
            if (foundChoices.Any())
            {
                var choices = foundChoices.Select(
                    choice =>
                    {
                        var (_, customVertex, text) = choice;
                        var choiceBtn = InlineKeyboardButton.WithCallbackData(text, FormatSceneId(customVertex.Id));
                        var choiceRow = new[] { choiceBtn };

                        return choiceRow;
                    });
                var choicesKeyboard = new InlineKeyboardMarkup(choices);

                await _botClient.EditMessageTextAsync(
                    callbackQuery.From.Id,
                    callbackQuery.Message.MessageId,
                    currentSceneDescription.Text,
                    replyMarkup: choicesKeyboard);
            }
            else
            {
                await _botClient.EditMessageTextAsync(
                    callbackQuery.From.Id,
                    callbackQuery.Message.MessageId,
                    currentSceneDescription.Text,
                    replyMarkup: RestartDialogInlineKeyboard);
            }
        }

        private async Task<Message> SendMenuButtonsAsync(Message message)
        {
            await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var wasStartedDialog = _dialogSystemCache.TryGetValue(message.From.Id.ToString(), out _);
            if (wasStartedDialog)
                return await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    TitleMode,
                    replyMarkup: SceneWasUploadedInlineKeyboard);

            return await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                TitleMode,
                replyMarkup: EmptyFileInlineKeyboard);
        }
    }
}