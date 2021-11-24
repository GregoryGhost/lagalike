namespace Lagalike.Demo.TestPatrickStar.MVU.Models
{
    using System;

    using PatrickStar.MVU;

    public record Model : IModel
    {
        public int CurrentNumber { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.DefaultModel;
    }
}