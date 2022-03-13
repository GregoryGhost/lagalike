namespace ThingsStore.Services
{
    using Lagalike.Telegram.Shared.Contracts;

    public class ModeSystem : BaseModeSystem
    {
        public ModeSystem(ThingsStoreModeInfo modeInfo, ThingsStoreHandleUpdateService updateHandler)
            : base(modeInfo.ModeInfo, updateHandler)
        {
        }
    }
}