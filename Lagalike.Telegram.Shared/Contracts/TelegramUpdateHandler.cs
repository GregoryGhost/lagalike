namespace Lagalike.Telegram.Shared.Contracts
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    /// <summary>
    /// Contract to a demo knowns how to handle a telegram update.
    /// </summary>
    public interface ITelegramUpdateHandler
    {
        /// <summary>
        /// Handle a telegram update.
        /// </summary>
        /// <param name="update">A Telegram update.</param>
        /// <returns>Returns awaition of handling.</returns>
        Task HandleUpdateAsync(Update update);
    }
}