namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using global::PatrickStar.MVU;

    using global::Telegram.Bot.Types;

    public abstract record TelegramUpdate : IUpdate
    {
        /// <inheritdoc />
        public string ChatId { get; init; } = null!;

        public Update Update { get; init; } = null!;

        public virtual RequestTypes RequestType { get; init; }
    }

    public record TelegramEditedMsgUpdate : TelegramUpdate
    {
        public override RequestTypes RequestType => RequestTypes.EditedMessage;
    }
    
    public record TelegramCallbackDataUpdate : TelegramUpdate
    {
        public override RequestTypes RequestType => RequestTypes.CallbackData;
    }
    
    public record TelegramMsgUpdate : TelegramUpdate
    {
        public override RequestTypes RequestType => RequestTypes.Message;
    }
}