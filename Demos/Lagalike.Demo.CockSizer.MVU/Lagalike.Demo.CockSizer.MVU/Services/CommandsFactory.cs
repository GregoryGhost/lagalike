namespace Lagalike.Demo.CockSizer.MVU.Services
{
    using System.Collections.Generic;

    using Lagalike.Demo.CockSizer.MVU.Commands;

    using PatrickStar.MVU;

    /// <summary>
    /// The demo commands factory.
    /// </summary>
    public class CommandsFactory
    {
        private static readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> Commands =
            new Dictionary<CommandTypes, ICommand<CommandTypes>>
            {
                { CommandTypes.GetCockSize, new GetCockSizeCommand() },
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
        public ICommand<CommandTypes> GetCockSizeCommand => Commands[CommandTypes.GetCockSize];

        /// <summary>
        /// Menu command.
        /// </summary>
        public ICommand<CommandTypes> MenuCommand => Commands[CommandTypes.Menu];
    }
}