namespace Lagalike.Demo.CockSizer.MVU.Models
{
    using System;

    using Lagalike.Demo.TestPatrickStar.MVU.Models;

    using PatrickStar.MVU;

    /// <summary>
    /// Data model.
    /// </summary>
    public record Model : IModel
    {
        /// <summary>
        /// Current user cock size for a user demo session.
        /// </summary>
        public byte? CockSize { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.DefaultModel;
    }
}