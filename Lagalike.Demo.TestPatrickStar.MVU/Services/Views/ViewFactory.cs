namespace Lagalike.Demo.TestPatrickStar.MVU.Services.Views
{
    using System.Collections.Generic;

    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    public class ViewFactory
    {
        private readonly IDictionary<ModelTypes, IViewMapper<CommandTypes>> _views;

        public ViewFactory(DefaultViewMapper defaultViewMapper)
        {
            _views = new Dictionary<ModelTypes, IViewMapper<CommandTypes>>
            {
                { ModelTypes.DefaultModel, defaultViewMapper }
            };
        }

        public IDictionary<ModelTypes, IViewMapper<CommandTypes>> GetViews()
        {
            return _views;
        }
    }
}