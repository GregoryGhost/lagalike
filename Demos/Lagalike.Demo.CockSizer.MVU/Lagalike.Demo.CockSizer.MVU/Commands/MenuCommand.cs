namespace Lagalike.Demo.CockSizer.MVU.Commands
{
    using PatrickStar.MVU;

    /// <summary>
    /// Command to return in the demo menu.
    /// </summary>
    public record MenuCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.Menu;
    }
}