namespace Lagalike.Demo.ThingsStoreSystem.MVU.Services.Views
{
    using System.Collections.Generic;

    using Lagalike.Demo.ThingsStoreSystem.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    /// <summary>
    /// A factory of the all demo views.
    /// </summary>
    public class ViewsFactory
    {
        private readonly IDictionary<ModelTypes, IViewMapper<CommandTypes>> _views;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="defaultViewMapper">A default view mapper of the demo.</param>
        public ViewsFactory(DefaultViewMapper defaultViewMapper)
        {
            _views = new Dictionary<ModelTypes, IViewMapper<CommandTypes>>
            {
                { ModelTypes.DefaultModel, defaultViewMapper }
            };
        }

        /// <summary>
        /// Get available demo views.
        /// </summary>
        /// <returns>Returns available demo views.</returns>
        public IDictionary<ModelTypes, IViewMapper<CommandTypes>> GetViews()
        {
            return _views;
        }
    }
}