namespace Lagalike.Demo.DialogSystem.Services
{
    using Lagalike.Telegram.Shared.Contracts;

    public class ModeSystem : BaseModeSystem
    {
        public ModeSystem(DialogModeInfo modeInfo, HandleUpdateService updateHandler)
            : base(modeInfo.ModeInfo, updateHandler)
        {
        }
    }
}