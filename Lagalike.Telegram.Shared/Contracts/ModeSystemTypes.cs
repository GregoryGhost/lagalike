namespace Lagalike.Telegram.Shared.Contracts
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    /// <summary>
    ///     A contract for mode system of the Lagalike sandbox.
    /// </summary>
    public interface IModeSystem
    {
        ModeInfo Info { get; }

        Task HandleUpdateAsync(Update update);
    }

    public interface IBackedModeSystem
    {
        /// <summary>
        ///     Gets the startup class of the module.
        /// </summary>
        public IStartup Startup { get; }
    }

    public abstract class BaseModeSystem : IModeSystem
    {
        private readonly ITelegramUpdateHandler _updateHandler;

        public BaseModeSystem(ModeInfo modeInfo, ITelegramUpdateHandler updateHandler)
        {
            Info = modeInfo;
            _updateHandler = updateHandler;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            await _updateHandler.HandleUpdateAsync(update);
        }

        public ModeInfo Info { get; }
    }

    /// <summary>
    ///     An information about a mode system.
    /// </summary>
    /// <param name="Name">A name of mode system.</param>
    public record ModeInfo(string Name, string AboutDescription, string ShortDescription);
}