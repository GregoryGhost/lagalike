namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using System.ComponentModel;

    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;
    using Lagalike.Demo.TestPatrickStar.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;

    public class HandleUpdateService : BaseMvuUpdateHandlerService<Model, ViewMapper, CommandTypes>
    {
        /// <inheritdoc />
        public HandleUpdateService(DataFlowManager dataFlowManager)
            : base(dataFlowManager)
        {
        }
    }
}