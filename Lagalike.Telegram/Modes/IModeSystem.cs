namespace Lagalike.Telegram.Modes
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    /// <summary>
    /// A contract for mode system of the Lagalike sandbox.
    /// </summary>
    public interface IModeSystem
    {
        ModeInfo GetInfo();
        Task GetHandlerUpdateAsync(Update update);
    }

    /// <summary>
    ///     An information about a mode system.
    /// </summary>
    /// <param name="Name">A name of mode system.</param>
    public record ModeInfo(string Name);
}