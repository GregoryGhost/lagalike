namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using System.Collections.Generic;

    using Lagalike.Demo.TestPatrickStar.MVU.Commands;

    using PatrickStar.MVU;

    public class CommandsFactory
    {
        private static readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> Commands =
            new Dictionary<CommandTypes, ICommand<CommandTypes>>
            {
                { CommandTypes.Increment, new IncrementCommand() },
                { CommandTypes.Decrement, new DecrementCommand() },
                { CommandTypes.Reset, new ResetCommand() },
                { CommandTypes.Menu, new MenuCommand() }
            };

        public IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> GetCommands()
        {
            return Commands;
        }

        public ICommand<CommandTypes> GetDecrementCommand()
        {
            return Commands[CommandTypes.Decrement];
        }

        public ICommand<CommandTypes> GetIncrementCommand()
        {
            return Commands[CommandTypes.Increment];
        }

        public ICommand<CommandTypes> GetResetCommand()
        {
            return Commands[CommandTypes.Reset];
        }

        public ICommand<CommandTypes> GetMenuCommand()
        {
            return Commands[CommandTypes.Menu];
        }
    }
}