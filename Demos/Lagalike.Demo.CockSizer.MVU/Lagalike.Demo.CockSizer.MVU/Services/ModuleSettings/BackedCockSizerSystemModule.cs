namespace Lagalike.Demo.CockSizer.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    /// <inheritdoc />
    public class BackedCockSizerSystemModule : IBackedModeSystem
    {
        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        public BackedCockSizerSystemModule()
        {
            Startup = new Startup();
        }

        /// <summary>
        /// Startup the demo with configured services.
        /// </summary>
        public IStartup Startup { get; }
    }
}