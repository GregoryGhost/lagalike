namespace Lagalike.Demo.TestPatrickStar.MVU.Commands
{
    using PatrickStar.MVU;

    public record ResetCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Reset;
    }
}