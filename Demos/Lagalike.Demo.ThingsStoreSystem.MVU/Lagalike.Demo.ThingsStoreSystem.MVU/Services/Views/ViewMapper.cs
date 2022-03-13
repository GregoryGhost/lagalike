namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services.Views
{
    using Lagalike.Demo.ThingsStoreSystem.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class ViewMapper : BaseMainViewMapper<CommandTypes, ModelTypes>
    {
        /// <inheritdoc />
        public ViewMapper(ViewsFactory viewsFactory)
            : base(viewsFactory.GetViews())
        {
        }
    }
}