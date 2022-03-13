namespace Lagalike.Demo.TestPatrickStar.MVU.Services.Views
{
    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
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