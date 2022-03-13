namespace Lagalike.Demo.ThingsStoreSystem.MVU.Commands
{
    using PatrickStar.MVU;

    /// <summary>
    /// Command to increment a value of current number for session.
    /// </summary>
    public record IncrementCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Increment;
    }
}