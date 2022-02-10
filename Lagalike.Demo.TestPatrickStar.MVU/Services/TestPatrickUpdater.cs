namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using System;
    using System.Threading.Tasks;

    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    /// <inheritdoc />
    public class TestPatrickUpdater : IUpdater<CommandTypes>
    {
        /// <inheritdoc />
        public async Task<(ICommand<CommandTypes>? OutputCommand, IModel UpdatedModel)> UpdateAsync(ICommand<CommandTypes> command, IModel model)
        {
            var castedModel = model switch
            {
                Model m1 => m1,
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(model)} type: {model}")
            };
            var updatedModel = command.Type switch
            {
                CommandTypes.Increment => castedModel with
                {
                    CurrentNumber = castedModel.CurrentNumber + 1
                },
                CommandTypes.Decrement => castedModel with
                {
                    CurrentNumber = castedModel.CurrentNumber - 1
                },
                CommandTypes.Reset => castedModel with
                {
                    CurrentNumber = 0
                },
                CommandTypes.Menu => castedModel,
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}") 
            };
            ICommand<CommandTypes> emptyCmd = null!;

            return (emptyCmd, updatedModel); 
        }
    }
}