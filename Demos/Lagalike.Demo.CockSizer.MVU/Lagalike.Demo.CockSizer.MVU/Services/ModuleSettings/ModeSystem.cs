namespace Lagalike.Demo.CockSizer.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    /// <inheritdoc />
    public class ModeSystem : BaseModeSystem
    {
        /// <inheritdoc />
        public ModeSystem(CockSizerInfo modeInfo, HandleUpdateService updateHandler)
            : base(modeInfo.ModeInfo, updateHandler)
        {
        }
    }
}