namespace Lagalike.Demo.ThingsStoreSystem.MVU.Commands
{
    /// <summary>
    /// Available commands types in the demo.
    /// </summary>
    public enum CommandTypes
    {
        /// <summary>
        /// Increment a value of current number for session.
        /// </summary>
        Increment,

        /// <summary>
        /// Decrement a value of current number for session.
        /// </summary>
        Decrement,

        /// <summary>
        /// Reset a value of current number for session.
        /// </summary>
        Reset,

        /// <summary>
        /// Show the demo menu.
        /// </summary>
        Menu
    }
}