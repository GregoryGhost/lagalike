namespace Lagalike.Demo.MVU.Models
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
        /// Current number for a user demo session.
        /// </summary>
        public int CurrentNumber { get; init; }

        /// <inheritdoc />
        public Enum Type => ModelTypes.DefaultModel;
    }
}