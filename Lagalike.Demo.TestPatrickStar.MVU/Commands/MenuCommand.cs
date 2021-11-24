namespace Lagalike.Demo.TestPatrickStar.MVU.Commands
{
    using PatrickStar.MVU;

    public record MenuCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Menu;
    }
}