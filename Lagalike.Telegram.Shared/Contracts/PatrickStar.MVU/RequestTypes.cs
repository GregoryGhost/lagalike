namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    /// <summary>
    /// Processed Telegram request types.
    /// </summary>
    public enum RequestTypes
    {
        /// <summary>
        /// Callback data (commands).
        /// </summary>
        CallbackData,
        
        /// <summary>
        /// A Telegram message from a user.
        /// </summary>
        Message,
        
        /// <summary>
        /// A Telegram edited message from a user.
        /// </summary>
        EditedMessage
    }
}