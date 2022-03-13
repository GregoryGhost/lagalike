namespace Lagalike.Telegram.Shared.Contracts
{
    using System.Threading.Tasks;

    using global::Telegram.Bot.Types;

    /// <summary>
    ///     A contract for mode system of the Lagalike sandbox.
    /// </summary>
    public interface IModeSystem
    {
        /// <summary>
        /// An information about a demo (a mode system).
        /// </summary>
        ModeInfo Info { get; }

        /// <summary>
        /// Handle a Telegram update.
        /// </summary>
        /// <param name="update">A Telegram update.</param>
        /// <returns>Returns awaition of handling.</returns>
        Task HandleUpdateAsync(Update update);
    }

    /// <summary>
    /// Requirements to implement module (mode/demo).
    /// </summary>
    public interface IBackedModeSystem
    {
        /// <summary>
        ///     Gets the startup class of the module.
        /// </summary>
        public IStartup Startup { get; }
    }

    /// <summary>
    /// Base implementation of mode 
    /// </summary>
    public abstract class BaseModeSystem : IModeSystem
    {
        private readonly ITelegramUpdateHandler _updateHandler;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="modeInfo">An information about a demo.</param>
        /// <param name="updateHandler">A demo Telegram update handler.</param>
        protected BaseModeSystem(ModeInfo modeInfo, ITelegramUpdateHandler updateHandler)
        {
            Info = modeInfo;
            _updateHandler = updateHandler;
        }

        /// <inheritdoc />
        public async Task HandleUpdateAsync(Update update)
        {
            await _updateHandler.HandleUpdateAsync(update);
        }

        /// <inheritdoc />
        public ModeInfo Info { get; }
    }

    /// <summary>
    ///     An information about a mode system.
    /// </summary>
    /// <param name="Name">A name of mode system.</param>
    public record ModeInfo(string Name, string AboutDescription, string ShortDescription);
}