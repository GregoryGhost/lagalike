namespace Lagalike.Demo.TestPatrickStar.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    public class BackedTestPatrickStarSystemModule : IBackedModeSystem
    {
        public BackedTestPatrickStarSystemModule()
        {
            Startup = new Startup();
        }

        public IStartup Startup { get; }
    }
}