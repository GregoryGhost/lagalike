namespace Lagalike.Demo.DialogSystem.Services
{
    using Lagalike.Telegram.Shared.Contracts;

    public class DialogModeInfo
    {
        private const string ABOUT_DESCRIPTION =
            "A demo of a dialog system based on the GrahpML format file.\n The GraphML format files you can create in the https://www.yworks.com/products/yed.";

        public const string MODE_NAME = "dialog";

        private const string SHORT_DESCRIPTION = "a demo of a simple dialog system";

        public readonly ModeInfo ModeInfo = new(
            MODE_NAME,
            ABOUT_DESCRIPTION,
            SHORT_DESCRIPTION);
    }
}