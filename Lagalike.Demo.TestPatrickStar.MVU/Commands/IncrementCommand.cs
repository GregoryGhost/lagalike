namespace Lagalike.Demo.TestPatrickStar.MVU.Commands
{
    using PatrickStar.MVU;

    public record IncrementCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Increment;
    }
}