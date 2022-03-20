namespace Lagalike.Demo.CockSizer.MVU.Services
{
    using Lagalike.Demo.CockSizer.MVU.Commands;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;
    using Lagalike.Telegram.Shared.Services;

    /// <inheritdoc />
    public class CockSizerPostProccessor : TelegramPostProccessor<CommandTypes>
    {
        /// <inheritdoc />
        public CockSizerPostProccessor(ConfiguredTelegramBotClient client)
            : base(client)
        {
        }
    }
}