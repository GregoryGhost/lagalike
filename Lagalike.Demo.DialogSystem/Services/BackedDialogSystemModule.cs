namespace Lagalike.Telegram.Modes
{
    public class BackedDialogSystemModule : IBackedModeSystem
    {
        public BackedDialogSystemModule()
        {
            Startup = new Startup();
        }

        public IStartup Startup { get; }
    }
}