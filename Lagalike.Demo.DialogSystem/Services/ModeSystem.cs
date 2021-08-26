namespace Lagalike.Telegram.Modes
{
    public class ModeSystem : BaseModeSystem
    {
        private const string ABOUT_DESCRIPTION =
            "A demo of a dialog system based on the GrahpML format file.\n The GraphML format files you can create in the https://www.yworks.com/products/yed.";

        public const string MODE_NAME = "dialog";

        private const string SHORT_DESCRIPTION = "a demo of a simple dialog system";

        private static readonly ModeInfo ModeInfo = new (
            MODE_NAME,
            ABOUT_DESCRIPTION,
            SHORT_DESCRIPTION);

        public ModeSystem(ITelegramUpdateHandler updateHandler)
            : base(ModeInfo, updateHandler)
        {
        }
    }
}