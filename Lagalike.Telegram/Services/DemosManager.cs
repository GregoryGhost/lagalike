namespace Lagalike.Telegram.Services
{
    using System.Linq;

    using Lagalike.Telegram.Shared.Contracts;

    public class DemosManager
    {
        private readonly string _availableDemosUsage;

        private readonly DemoRegistrator _registrator;

        public DemosManager(DemoRegistrator registrator)
        {
            _registrator = registrator;
            _availableDemosUsage = FormatUsageAvailableDemoCommands();
        }

        public IModeSystem? GetByName(string demoName)
        {
            return _registrator.GetRegistratedModule(demoName);
        }

        public string GetDemosUsage()
        {
            return _availableDemosUsage;
        }

        private static string FormatDemoUsage(IModeSystem demoSystem)
        {
            var (demoName, _, shortDescription) = demoSystem.Info;
            var demoCmd = $"/{demoName}";
            var demoUsage = $"{demoCmd} - {shortDescription}";

            return demoUsage;
        }

        private string FormatUsageAvailableDemoCommands()
        {
            var demoInfos = _registrator.GetRegistratedModules().Select(FormatDemoUsage);
            var availableCmds = string.Join("\n", demoInfos);
            var usage = $"Usage:\n{availableCmds}";

            return usage;
        }
    }
}