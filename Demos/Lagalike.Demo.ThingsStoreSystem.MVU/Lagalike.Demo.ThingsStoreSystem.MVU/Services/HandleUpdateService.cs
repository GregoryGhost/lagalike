namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services
{
    using Lagalike.Demo.ThingsStoreSystem.MVU.Commands;
    using Lagalike.Demo.ThingsStoreSystem.MVU.Models;
    using Lagalike.Demo.ThingsStoreSystem.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;

    /// <inheritdoc />
    public class HandleUpdateService : BaseMvuUpdateHandlerService<Model, ViewMapper, CommandTypes>
    {
        /// <inheritdoc />
        public HandleUpdateService(DataFlowManager dataFlowManager)
            : base(dataFlowManager)
        {
        }
    }
}