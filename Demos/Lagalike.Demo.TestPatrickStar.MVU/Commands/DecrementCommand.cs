namespace Lagalike.Demo.TestPatrickStar.MVU.Commands
{
    using PatrickStar.MVU;

    /// <summary>
    /// Command to decrement a value of current number for session.
    /// </summary>
    public record DecrementCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Decrement;
    }
}