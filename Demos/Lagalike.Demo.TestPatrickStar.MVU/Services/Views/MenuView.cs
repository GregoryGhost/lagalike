namespace Lagalike.Demo.TestPatrickStar.MVU.Services.Views
{
    using Lagalike.Demo.TestPatrickStar.MVU.Commands;

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
            Menu = new MenuBuilder<CommandTypes>()
                   .Row()
                   .Button("+1", commandsFactory.IncrementCommand)
                   .Button("-1", commandsFactory.DecrementCommand)
                   .Button("Reset", commandsFactory.ResetCommand)
                   .Build("Current value: 0");
            _initialMenu = new MenuBuilder<CommandTypes>()
                           .Row()
                           .Button("+1", commandsFactory.IncrementCommand)
                           .Button("-1", commandsFactory.DecrementCommand)
                           .Build("Current value: 0");
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
        /// <param name="newNumber">A new number for current value.</param>
        /// <returns>Return a updated view.</returns>
        public IView<CommandTypes> UpdateMenu(int newNumber)
        {
            var msg = Menu.MessageElement with
            {
                Text = $"Current value: {newNumber}"
            };
            var menu = newNumber == 0
                ? _initialMenu
                : Menu;
            var updatedMenu = menu with
            {
                MessageElement = msg,
            };

            return Update(updatedMenu);
        }
    }
}