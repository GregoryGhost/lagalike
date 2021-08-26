namespace Lagalike.Telegram.Services
{
    using System.Linq;

    using Lagalike.Telegram.Modes;

    public class DemosManager
    {
        private readonly DemoRegistrator _registrator;

        private readonly string _availableDemosUsage;

        public DemosManager(DemoRegistrator registrator)
        {
            _registrator = registrator;
            _availableDemosUsage = FormatUsageAvailableDemoCommands();
        }

        public string GetDemosUsage()
        {
            return _availableDemosUsage;
        }

        private string FormatUsageAvailableDemoCommands()
        {
            var demoInfos = _registrator.GetRegistratedModules().Select(FormatDemoUsage);
            var availableCmds = string.Join("\n", demoInfos);
            var usage = $"Usage:\n{availableCmds}";

            return usage;
        }

        private static string FormatDemoUsage(IModeSystem demoSystem)
        {
            var (demoName, _, shortDescription) = demoSystem.Info;
            var demoCmd = $"/{demoName}";
            var demoUsage = $"{demoCmd} - {shortDescription}";

            return demoUsage;
        }

        public IModeSystem? GetByName(string demoName)
        {
            return _registrator.GetRegistratedModule(demoName);
        }
    }
}