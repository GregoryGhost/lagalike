namespace Lagalike.Demo.TestPatrickStar.MVU.Services.ModuleSettings
{
    using Lagalike.Telegram.Shared.Contracts;

    public class ModeSystem : BaseModeSystem
    {
        public ModeSystem(TestPatrickStarInfo modeInfo, HandleUpdateService updateHandler)
            : base(modeInfo.ModeInfo, updateHandler)
        {
        }
    }
}