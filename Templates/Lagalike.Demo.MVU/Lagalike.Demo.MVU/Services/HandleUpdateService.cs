namespace Lagalike.Demo.MVU.Services
{
    using Lagalike.Demo.MVU.Commands;
    using Lagalike.Demo.MVU.Models;
    using Lagalike.Demo.MVU.Services.Views;
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