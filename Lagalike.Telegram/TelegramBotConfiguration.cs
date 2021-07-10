namespace Telegram.Bot.Examples.WebHook.Services
{
    using System.ComponentModel.DataAnnotations;

    public record TelegramBotConfiguration
    {
        [Required] public string BotToken { get; init; }

        [Required] public string HostAddress { get; init; }
    }
}