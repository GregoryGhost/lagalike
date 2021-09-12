namespace ThingsStore.Services
{
    using Lagalike.Telegram.Modes;
    using Lagalike.Telegram.Shared.Contracts;

    public class BackedThingsStoreSystemModule : IBackedModeSystem
    {
        public BackedThingsStoreSystemModule()
        {
            Startup = new Startup();
        }

        public IStartup Startup { get; }
    }
}