namespace Lagalike.Telegram.Services
{
    using System.Collections.Generic;

    using Lagalike.Telegram.Shared.Contracts;

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

        public IModeSystem? GetRegistratedModule(string moduleName)
        {
            return _demos.TryGetValue(moduleName, out var foundModule) ? foundModule : null;
        }

        public IEnumerable<IModeSystem> GetRegistratedModules()
        {
            return _demos.Values;
        }

        private void Registrate(IModeSystem demo)
        {
            _demos.Add(demo.Info.Name, demo);
        }
    }
}