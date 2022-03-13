namespace PatrickStar.MVU
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A cache of a model.
    /// </summary>
    /// <typeparam name="TItem">A type of a model.</typeparam>
    public interface IModelCache<TItem>
    {
        /// <summary>
        ///     Set a value to a chat id.
        /// </summary>
        /// <param name="chatId">A chat id.</param>
        /// <param name="value">A saved value.</param>
        void Set(string chatId, TItem value);

        /// <summary>
        ///     Try to get a value from cache.
        /// </summary>
        /// <param name="chatId">A chat id.</param>
        /// <param name="value">A saved object value.</param>
        /// <returns>Returns "true" if a object value found else "false".</returns>
        bool TryGetValue(string chatId, [MaybeNullWhen(false)]out TItem value);
    }
}