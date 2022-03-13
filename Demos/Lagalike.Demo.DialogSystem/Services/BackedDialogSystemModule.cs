namespace Lagalike.Demo.DialogSystem.Services
{
    using Lagalike.Telegram.Modes;
    using Lagalike.Telegram.Shared.Contracts;

    public class BackedDialogSystemModule : IBackedModeSystem
    {
        public BackedDialogSystemModule()
        {
            Startup = new Startup();
        }

        public IStartup Startup { get; }
    }
}