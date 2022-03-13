namespace Lagalike.Demo.MVU.Services
{
    using System.Collections.Generic;

    using Lagalike.Demo.MVU.Commands;

    using PatrickStar.MVU;

    /// <summary>
    /// The demo commands factory.
    /// </summary>
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
        
        /// <summary>
        /// Get available commands of the demo.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> GetCommands()
        {
            return Commands;
        }
        
        /// <summary>
        /// Decrement command.
        /// </summary>
        public ICommand<CommandTypes> DecrementCommand => Commands[CommandTypes.Decrement];

        /// <summary>
        /// Increment command.
        /// </summary>
        public ICommand<CommandTypes> IncrementCommand => Commands[CommandTypes.Increment];

        /// <summary>
        /// Reset command.
        /// </summary>
        public ICommand<CommandTypes> ResetCommand => Commands[CommandTypes.Reset];

        /// <summary>
        /// Menu command.
        /// </summary>
        public ICommand<CommandTypes> MenuCommand => Commands[CommandTypes.Menu];
    }
}