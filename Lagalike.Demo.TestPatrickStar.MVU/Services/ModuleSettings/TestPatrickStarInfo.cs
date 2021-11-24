namespace Lagalike.Demo.TestPatrickStar.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    public class TestPatrickStarInfo
    {
        private const string ABOUT_DESCRIPTION =
            "A demo of using the MVU pattern in Telegram";

        public const string MODE_NAME = "test_mvu";

        private const string SHORT_DESCRIPTION = "a demo of a simple realization MVU pattern";

        public readonly ModeInfo ModeInfo = new(
            MODE_NAME,
            ABOUT_DESCRIPTION,
            SHORT_DESCRIPTION);
    }
}