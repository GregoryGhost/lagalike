namespace Lagalike.Telegram.Shared
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    ///     The Telegram bot configuration.
    /// </summary>
    public record TelegramBotConfiguration
    {
        /// <summary>
        ///     A section name in appsettings.
        /// </summary>
        public const string CONFIGURATION_SECTION_NAME = "TelegramBotConfiguration";

        /// <summary>
        ///     Telegram bot token.
        /// </summary>
        [Required]
        public string BotToken { get; init; } = null!;

        /// <summary>
        ///     Host address for a web hook mode.
        /// </summary>
        [Required]
        public string HostAddress { get; init; } = null!;
    }
}