namespace Lagalike.Demo.CockSizer.MVU.Services
{
    using Lagalike.Demo.CockSizer.MVU.Commands;
    using Lagalike.Demo.CockSizer.MVU.Models;
    using Lagalike.Demo.CockSizer.MVU.Services.Views;
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