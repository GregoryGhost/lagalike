namespace Lagalike.Demo.TestPatrickStar.MVU.Commands
{
    using PatrickStar.MVU;

    public record DecrementCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Decrement;
    }
}