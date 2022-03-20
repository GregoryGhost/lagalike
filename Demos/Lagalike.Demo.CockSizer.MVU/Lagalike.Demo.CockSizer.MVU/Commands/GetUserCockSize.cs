namespace Lagalike.Demo.CockSizer.MVU.Commands
{
    using PatrickStar.MVU;

    /// <summary>
    /// Command to return a user cock size.
    /// </summary>
    public record GetCockSizeCommand : BaseCommand<CommandTypes>
    {
        /// <inheritdoc />
        public override CommandTypes Type => CommandTypes.GetCockSize;
    }
}