namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    /// <summary>
    /// A information about this mode.
    /// </summary>
    public class TestPatrickStarInfo
    {
        private const string ABOUT_DESCRIPTION =
            "A demo of using the MVU pattern in Telegram";

        private const string MODE_NAME = "test_mvu";

        private const string SHORT_DESCRIPTION = "a demo of a simple realization MVU pattern";

        /// <summary>
        /// The mode information.
        /// </summary>
        public readonly ModeInfo ModeInfo = new(
            MODE_NAME,
            ABOUT_DESCRIPTION,
            SHORT_DESCRIPTION);
    }
}