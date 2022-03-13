namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    /// <inheritdoc />
    public class BackedTestPatrickStarSystemModule : IBackedModeSystem
    {
        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        public BackedTestPatrickStarSystemModule()
        {
            Startup = new Startup();
        }

        /// <summary>
        /// Startup the demo with configured services.
        /// </summary>
        public IStartup Startup { get; }
    }
}