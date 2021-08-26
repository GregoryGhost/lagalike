namespace Lagalike.Demo.DialogSystem.Constants
{
    using Lagalike.Telegram.Modes;

    /// <summary>
    ///     Available commands of the dialog system demo.
    /// </summary>
    internal static class AvailableDemoCommands
    {
        /// <summary>
        ///     Trigger for get full description about the demo mode.
        /// </summary>
        public static readonly CommandInfo About = new("About", "About");
        
        /// <summary>
        ///     Trigger for get full description about the demo mode.
        /// </summary>
        public static readonly CommandInfo Start = new("Start", StartCommandValue);
        
        /// <summary>
        ///     Trigger for get full description about the demo mode.
        /// </summary>
        public static readonly CommandInfo Restart = new("Restart", StartCommandValue);
        
        /// <summary>
        ///     Trigger for get full description about the demo mode.
        /// </summary>
        public static readonly CommandInfo ProccessNextScene = new("Next scene", $"{ModeSystem.MODE_NAME} next scene");

        private static readonly string StartCommandValue = $"{ModeSystem.MODE_NAME} start";
    }
}