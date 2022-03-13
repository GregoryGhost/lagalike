namespace Lagalike.Telegram.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using global::Telegram.Bot.Exceptions;
    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;

    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Services;

    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    public class HandleUpdateService
    {
        private readonly string _availableDemosUsage;

        private readonly ConfiguredTelegramBotClient _botClient;

        private readonly TelegramConversationCache _conversationCache;

        private readonly DemosManager _demosManager;

        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(ConfiguredTelegramBotClient botClient, ILogger<HandleUpdateService> logger,
            TelegramConversationCache conversationCache, DemosManager demosManager)
        {
            _botClient = botClient;
            _logger = logger;
            _conversationCache = conversationCache;
            _demosManager = demosManager;
            _availableDemosUsage = _demosManager.GetDemosUsage();
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogCritical(errorMessage);

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

        private ConversationState? CacheDemoState(string chatId, string parsedCmd)
        {
            var parsedDemoName = parsedCmd.Replace("/", "");
            var demo = _demosManager.GetByName(parsedDemoName);
            if (demo is null)
                return null;

            var demoState = new ConversationState(demo);
            _conversationCache.Set(
                chatId,
                demoState
            );

            return demoState;
        }

        private static string GetChatId(Update telegramUserUpdate)
        {
            if (telegramUserUpdate.Message is not null)
                return telegramUserUpdate.Message.From.Id.ToString();
            if (telegramUserUpdate.CallbackQuery is not null)
                return telegramUserUpdate.CallbackQuery.From.Id.ToString();

            throw new InvalidOperationException(
                $"Cannot get a chat id for a update: {JsonConvert.SerializeObject(telegramUserUpdate)}");
        }

        private async Task GetHandlerAsync(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message or UpdateType.EditedMessage or UpdateType.CallbackQuery => HandleDemoUpdaterAsync(update),
                _ => UnknownUpdateHandlerAsync(update)
            };

            await handler;
        }

        private async Task HandleDemoUpdaterAsync(Update telegramUserUpdate)
        {
            var currentTelegramUserDemo = TryGetCurrentTelegramUserDemo(telegramUserUpdate);
            if (currentTelegramUserDemo is not null)
            {
                await currentTelegramUserDemo.SelectedDemoMode.HandleUpdateAsync(telegramUserUpdate);
            }
            else
            {
                _logger.LogWarning($"Gotten an unknown demo mode for {JsonConvert.SerializeObject(telegramUserUpdate)}");

                await Usage(telegramUserUpdate);
            }
        }

        private ConversationState? TryGetCurrentTelegramUserDemo(Update telegramUserUpdate)
        {
            var chatId = GetChatId(telegramUserUpdate);
            if (_conversationCache.TryGetValue(chatId, out var telegramUserData))
                return telegramUserData;

            if (telegramUserUpdate.Message?.Type != MessageType.Text)
                return null;

            var parsedCmd = telegramUserUpdate.Message.Text.Split(' ').First();
            var cachedDemoState = CacheDemoState(chatId, parsedCmd);

            return cachedDemoState;
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        private async Task Usage(Update telegramUpdate)
        {
            var chatId = GetChatId(telegramUpdate);

            await _botClient.SendTextMessageAsync(
                chatId,
                _availableDemosUsage);
        }
    }

    public class TelegramConversationCache : BaseTelegramBotCache<ConversationState>
    {
        private const string BOT_CACHE_NAME = "botCache";

        /// <inheritdoc />
        public TelegramConversationCache(IMemoryCache telegramCache)
            : base(telegramCache, BOT_CACHE_NAME)
        {
        }
    }

    public record ConversationState(IModeSystem SelectedDemoMode);
}