namespace Lagalike.Demo.DialogSystem.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CSharpFunctionalExtensions;

    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;
    using global::Telegram.Bot.Types.InputFiles;
    using global::Telegram.Bot.Types.ReplyMarkups;

    using Lagalike.Demo.DialogSystem.Constants;
    using Lagalike.GraphML.Parser;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Services;

    public class HandleUpdateService : ITelegramUpdateHandler
    {
        private const string EXAMPLE_SCENE_FILE_NAME = "scenes-choices.graphml";

        private const string UPLOAD_TOOLTIP = "You can upload a GraphML file at any time.";

        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly Loader _dialogLoader;

        private readonly DialogSystemCache _dialogSystemCache;

        private readonly InlineKeyboardMarkup _emptyFileInlineKeyboard;

        private readonly InputOnlineFile _exampleSceneFile;

        private readonly ModeInfo _modeInfo;

        private readonly InlineKeyboardMarkup _restartDialogInlineKeyboard;

        private readonly InlineKeyboardMarkup _sceneWasUploadedInlineKeyboard;

        private readonly string _titleMode;

        private readonly string _wasSuccessfullyProccessedSceneDocument;

        public HandleUpdateService(ConfiguredTelegramBotClient botClient, Loader dialogLoader,
            DialogSystemCache dialogSystemCache,
            DialogModeInfo modeInfo)
        {
            _botClient = botClient;
            var aboutModeButton = GetInlineKeyboardButton(AvailableDemoCommands.About);
            var restartButton = GetInlineKeyboardButton(AvailableDemoCommands.Restart);
            var startButton = GetInlineKeyboardButton(AvailableDemoCommands.Start);
            var downloadExampleSceneFileButton = GetInlineKeyboardButton(AvailableDemoCommands.DownloadExampleSceneFile);
            _titleMode = $"Dialog system. {UPLOAD_TOOLTIP}";
            _wasSuccessfullyProccessedSceneDocument = $"The document was successfully proccessed.\n {_titleMode}";
            _sceneWasUploadedInlineKeyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[] { startButton },
                    new[] { downloadExampleSceneFileButton },
                    new[] { aboutModeButton }
                }
            );
            _emptyFileInlineKeyboard = new InlineKeyboardMarkup(
                new[]
                {
                    new[] { downloadExampleSceneFileButton },
                    new[] { aboutModeButton }
                });
            _restartDialogInlineKeyboard = new InlineKeyboardMarkup(
                restartButton
            );
            _dialogLoader = dialogLoader;
            _dialogSystemCache = dialogSystemCache;
            _modeInfo = modeInfo.ModeInfo;

            _exampleSceneFile = GetExampleSceneFile();
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

        private static string FormatSceneId(string targetId)
        {
            return $"{DialogModeInfo.MODE_NAME} next scene id{targetId}";
        }

        private static InputOnlineFile GetExampleSceneFile()
        {
            var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var exampleSceneFilePath = Path.Combine(appPath, EXAMPLE_SCENE_FILE_NAME);
            return new InputOnlineFile(new FileStream(exampleSceneFilePath, FileMode.Open), EXAMPLE_SCENE_FILE_NAME);
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

            var currentDocument = new MemoryStream();
            await _botClient.DownloadFileAsync(file.FilePath, currentDocument);

            return currentDocument;
        }

        private static bool IsCommandToProccessNextSceneDialog(string callbackQueryData)
        {
            return callbackQueryData.Contains(AvailableDemoCommands.ProccessNextScene.CommandValue);
        }

        private static bool IsCommandToProccessSceneDialog(CallbackQuery callbackQuery)
        {
            return callbackQuery.Data.Contains(DialogModeInfo.MODE_NAME);
        }

        private async Task ParseSceneFile(Message message, MemoryStream currentDocument)
        {
            var parsedScenesGraph = _dialogLoader.ParseFile(currentDocument);

            if (parsedScenesGraph.IsSuccess)
            {
                _dialogSystemCache.Set(message.Chat.Id.ToString(), parsedScenesGraph.Value);

                await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    _wasSuccessfullyProccessedSceneDocument,
                    replyMarkup: _sceneWasUploadedInlineKeyboard);
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

            var currentDocument = await GetSceneFileAsync(message);

            await ParseSceneFile(message, currentDocument);
        }

        private async Task ProccessInlineKeyboardCallbackDataAsync(CallbackQuery callbackQuery)
        {
            var msg = "It's nothing.";
            if (callbackQuery.Data == AvailableDemoCommands.About.CommandValue)
                msg = _modeInfo.AboutDescription;

            if (IsCommandToProccessSceneDialog(callbackQuery))
            {
                await ProccessSceneDialogAsync(callbackQuery);
                return;
            }

            if (callbackQuery.Data == AvailableDemoCommands.DownloadExampleSceneFile.CommandValue)
            {
                await SendExampleSceneFileAsync(callbackQuery);
                return;
            }

            await _botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                msg);
        }

        private async Task ProccessReceivedMessageAsync(Message message)
        {
            if (message.Type == MessageType.Document)
                await ProccessDocumentAsync(message);

            if (message.Type == MessageType.Text)
                await SendMenuButtonsAsync(message);
        }

        private async Task ProccessSceneDialogAsync(CallbackQuery callbackQuery)
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
                    replyMarkup: _restartDialogInlineKeyboard);
            }
        }

        private async Task SendExampleSceneFileAsync(CallbackQuery callbackQuery)
        {
            var chatId = callbackQuery.From.Id.ToString();

            await _botClient.SendChatActionAsync(chatId, ChatAction.UploadDocument);

            const string TooltipUseFile = "It's example scene file. You can send the file to the bot.";
            await _botClient.SendDocumentAsync(chatId, _exampleSceneFile, caption: TooltipUseFile);
        }

        private async Task<Message> SendMenuButtonsAsync(Message message)
        {
            await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var wasStartedDialog = _dialogSystemCache.TryGetValue(message.From.Id.ToString(), out _);
            if (wasStartedDialog)
                return await _botClient.SendTextMessageAsync(
                    message.Chat.Id,
                    _titleMode,
                    replyMarkup: _sceneWasUploadedInlineKeyboard);

            return await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                _titleMode,
                replyMarkup: _emptyFileInlineKeyboard);
        }
    }
}