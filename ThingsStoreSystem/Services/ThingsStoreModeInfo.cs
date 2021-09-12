namespace ThingsStore.Services
{
    using Lagalike.Telegram.Shared.Contracts;

    public class ThingsStoreModeInfo
    {
        private const string ABOUT_DESCRIPTION =
            "A demo of a simple things store.";

        public const string MODE_NAME = "things_store";

        private const string SHORT_DESCRIPTION = "a demo of a simple things store.";

        public readonly ModeInfo ModeInfo = new(
            MODE_NAME,
            ABOUT_DESCRIPTION,
            SHORT_DESCRIPTION);
    }
}