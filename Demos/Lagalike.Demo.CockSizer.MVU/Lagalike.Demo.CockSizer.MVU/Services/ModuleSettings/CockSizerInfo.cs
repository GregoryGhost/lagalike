namespace Lagalike.Demo.CockSizer.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    /// <summary>
    /// A information about this mode.
    /// </summary>
    public class CockSizerInfo
    {
        private const string ABOUT_DESCRIPTION =
            "A demo of share a user cock size.";

        private const string MODE_NAME = "cock_sizer";

        private const string SHORT_DESCRIPTION = "a demo of sharing a user cock size.";

        /// <summary>
        /// The mode information.
        /// </summary>
        public readonly ModeInfo ModeInfo = new(
            MODE_NAME,
            ABOUT_DESCRIPTION,
            SHORT_DESCRIPTION);
    }
}