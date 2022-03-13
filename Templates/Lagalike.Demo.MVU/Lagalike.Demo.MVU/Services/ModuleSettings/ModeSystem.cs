namespace Lagalike.Demo.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    /// <inheritdoc />
    public class ModeSystem : BaseModeSystem
    {
        /// <inheritdoc />
        public ModeSystem(TestPatrickStarInfo modeInfo, HandleUpdateService updateHandler)
            : base(modeInfo.ModeInfo, updateHandler)
        {
        }
    }
}