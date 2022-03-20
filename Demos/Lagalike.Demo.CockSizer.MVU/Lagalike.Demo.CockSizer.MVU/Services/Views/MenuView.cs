namespace Lagalike.Demo.CockSizer.MVU.Services.Views
{
    using Lagalike.Demo.CockSizer.MVU.Commands;
    using Lagalike.Demo.CockSizer.MVU.Models;

    using PatrickStar.MVU;

    /// <summary>
    ///     A demo menu view.
    /// </summary>
    public record MenuView : BaseMenuView<CommandTypes>
    {
        private readonly Menu<CommandTypes> _initialMenu;

        /// <summary>
        ///     Initialize dependencies.
        /// </summary>
        /// <param name="commandsFactory">A factory which contains all commands of the mode.</param>
        public MenuView(CommandsFactory commandsFactory)
        {
            _initialMenu = new MenuBuilder<CommandTypes>()
                           .Row()
                           .Button("Get your cock size", commandsFactory.GetCockSizeCommand)
                           .Build("I don't know your cock size.");
            Menu = _initialMenu;
        }

        /// <inheritdoc />
        public override IView<CommandTypes> Update(Menu<CommandTypes> sourceMenu)
        {
            var view = this with
            {
                Menu = sourceMenu
            };

            return view;
        }

        /// <summary>
        ///     Update the view menu.
        /// </summary>
        /// <param name="model">A cock size model.</param>
        /// <returns>Return a updated view.</returns>
        public IView<CommandTypes> UpdateMenu(Model model)
        {
            if (model.CockSize is null)
            {
                return Update(_initialMenu);
            }
            
            var msg = Menu.MessageElement with
            {
                Text = $"Your cock size is {model.CockSize} cm"
            };
            
            var updatedMenu = Menu with
            {
                MessageElement = msg,
            };

            return Update(updatedMenu);
        }
    }
}