namespace Lagalike.Demo.ThingsStoreSystem.MVU.Commands
{
    using PatrickStar.MVU;

    /// <summary>
    /// Command to reset a value of current number for session.
    /// </summary>
    public record ResetCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Reset;
    }
}