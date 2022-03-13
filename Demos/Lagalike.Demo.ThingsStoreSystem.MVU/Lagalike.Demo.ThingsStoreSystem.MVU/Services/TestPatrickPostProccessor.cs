namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services
{
    using Lagalike.Demo.ThingsStoreSystem.MVU.Commands;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;
    using Lagalike.Telegram.Shared.Services;

    /// <inheritdoc />
    public class TestPatrickPostProccessor : TelegramPostProccessor<CommandTypes>
    {
        /// <inheritdoc />
        public TestPatrickPostProccessor(ConfiguredTelegramBotClient client)
            : base(client)
        {
        }
    }
}