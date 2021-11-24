namespace Lagalike.Demo.TestPatrickStar.MVU.Services.Views
{
    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    public class DefaultViewMapper : IViewMapper<CommandTypes>
    {
        private readonly IView<CommandTypes> _menuView;

        public DefaultViewMapper(MenuView menuView)
        {
            _menuView = menuView;
        }

        /// <inheritdoc />
        public IView<CommandTypes> Map(IModel model)
        {
            var defaultModel = (Model)model;
            var msg = _menuView.Menu.MessageElement with
            {
                Text = $"Current value: {defaultModel.CurrentNumber}"
            };
            var updatedMenu = _menuView.Menu with
            {
                MessageElement = msg
            };
            var menu = _menuView.Update(updatedMenu);

            return menu;
        }
    }
}