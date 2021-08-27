namespace Lagalike.Telegram.Shared.Contracts
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    public interface ITelegramUpdateHandler
    {
        Task HandleUpdateAsync(Update update);
    }
}