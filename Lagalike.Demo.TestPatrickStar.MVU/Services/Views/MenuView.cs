namespace Lagalike.Demo.TestPatrickStar.MVU.Services.Views
{
    using Lagalike.Demo.TestPatrickStar.MVU.Commands;

    using PatrickStar.MVU;

    /// <summary>
    /// A demo mene view.
    /// </summary>
    public record MenuView : BaseMenuView<CommandTypes>
    {
        private readonly CommandsFactory _commandsFactory;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="commandsFactory">A factory which contains all commands of the mode.</param>
        public MenuView(CommandsFactory commandsFactory)
        {
            _commandsFactory = commandsFactory;
            Menu = new MenuBuilder<CommandTypes>()
                   .Row()
                   .Button("+1", commandsFactory.IncrementCommand)
                   .Button("-1", commandsFactory.DecrementCommand)
                   .Button("Reset", commandsFactory.ResetCommand)
                   .Build("Current value: 0");
        }

        /// <inheritdoc />
        public override IView<CommandTypes> Update(Menu<CommandTypes> sourceMenu)
        {
            var updatedMenu = sourceMenu with
            {
                Buttons = Menu.Buttons
            };
            var view = new MenuView(_commandsFactory)
            {
                Menu = updatedMenu
            };
            
            return view;
        }
    }
}