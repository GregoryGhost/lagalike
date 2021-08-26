namespace Lagalike.Telegram.Modes
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    public interface ITelegramUpdateHandler
    {
        Task HandleUpdateAsync(Update update); 
    }
}