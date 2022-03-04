namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using System;
    using System.Threading.Tasks;

    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class TestPatrickUpdater : IUpdater<CommandTypes, Model>
    {
        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, Model UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command, Model model)
        {
            var updatedModel = command.Type switch
            {
                CommandTypes.Increment => model with
                {
                    CurrentNumber = model.CurrentNumber + 1
                },
                CommandTypes.Decrement => model with
                {
                    CurrentNumber = model.CurrentNumber - 1
                },
                CommandTypes.Reset => model with
                {
                    CurrentNumber = 0
                },
                CommandTypes.Menu => model,
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}") 
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel); 
        }
    }
}