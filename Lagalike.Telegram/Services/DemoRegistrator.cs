namespace Lagalike.Telegram.Services
{
    using System.Collections.Generic;

    using Lagalike.Telegram.Modes;

    public class DemoRegistrator
    {
        private readonly Dictionary<string, IModeSystem> _demos = new();

        public DemoRegistrator(IEnumerable<IModeSystem> availableDemos)
        {
            foreach (var availableDemo in availableDemos)
            {
                Registrate(availableDemo);
            }
        }

        private void Registrate(IModeSystem demo)
        {
            _demos.Add(demo.Info.Name, demo);
        }

        public IEnumerable<IModeSystem> GetRegistratedModules()
        {
            return _demos.Values;
        }

        public IModeSystem? GetRegistratedModule(string moduleName)
        {
            return _demos.TryGetValue(moduleName, out var foundModule) ? foundModule : null;
        }
    }
}