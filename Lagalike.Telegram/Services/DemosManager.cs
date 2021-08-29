namespace Lagalike.Telegram.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Telegram.Bot.Types;

    using Lagalike.Telegram.Shared.Contracts;

    public class DemosManager
    {
        private readonly IEnumerable<BotCommand> _availableBotCommands;

        private readonly string _availableDemosUsage;

        private readonly DemoRegistrator _registrator;

        public DemosManager(DemoRegistrator registrator)
        {
            _registrator = registrator;
            _availableDemosUsage = FormatUsageAvailableDemoCommands();
            _availableBotCommands = FormatBotCommands();
        }

        public IEnumerable<BotCommand> GetAvailableBotCommands()
        {
            return _availableBotCommands;
        }

        public IModeSystem? GetByName(string demoName)
        {
            return _registrator.GetRegistratedModule(demoName);
        }

        public string GetDemosUsage()
        {
            return _availableDemosUsage;
        }

        private IEnumerable<BotCommand> FormatBotCommands()
        {
            var demosCommands = _registrator.GetRegistratedModules()
                                            .Select(
                                                module => new BotCommand
                                                {
                                                    Command = module.Info.Name,
                                                    Description = module.Info.ShortDescription
                                                });

            return demosCommands;
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