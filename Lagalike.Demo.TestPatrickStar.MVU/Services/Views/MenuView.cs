namespace Lagalike.Demo.TestPatrickStar.MVU.Services.Views
{
    using Lagalike.Demo.TestPatrickStar.MVU.Commands;

    using PatrickStar.MVU;

    public record MenuView : BaseMenuView<CommandTypes>
    {
        private readonly CommandsFactory _commandsFactory;

        public MenuView(CommandsFactory commandsFactory)
        {
            _commandsFactory = commandsFactory;
            Menu = new MenuBuilder<CommandTypes>()
                   .Row()
                   .Button("+1", commandsFactory.GetIncrementCommand())
                   .Button("-1", commandsFactory.GetDecrementCommand())
                   .Button("Reset", commandsFactory.GetResetCommand())
                   .Build("Current value: 0");
        }

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