namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;
    using Lagalike.Telegram.Shared.Services;

    public class TestPatrickPostProccessor : TelegramPostProccessor<CommandTypes>
    {
        /// <inheritdoc />
        public TestPatrickPostProccessor(ConfiguredTelegramBotClient client)
            : base(client)
        {
        }
    }
}