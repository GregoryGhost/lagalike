namespace Lagalike.Demo.DialogSystem.Constants
{
    using Lagalike.Demo.DialogSystem.Services;

    /// <summary>
    ///     Available commands of the dialog system demo.
    /// </summary>
    internal static class AvailableDemoCommands
    {
        /// <summary>
        ///     Trigger to get full description about the demo mode.
        /// </summary>
        public static readonly CommandInfo About = new("About", "About");

        /// <summary>
        ///     Trigger to proccess a next scene in a current user session.
        /// </summary>
        public static readonly CommandInfo ProccessNextScene = new("Next scene", FormatProcessSceneCommandValue("next scene"));

        private static readonly string StartCommandValue = FormatProcessSceneCommandValue("start");

        /// <summary>
        ///     Trigger to download an example scene file to an user.
        /// </summary>
        public static readonly CommandInfo DownloadExampleSceneFile = new("Download an example scene file", "example scene file");

        /// <summary>
        ///     Trigger to restart proccessing a current scene .
        /// </summary>
        public static readonly CommandInfo Restart = new("Restart", StartCommandValue);

        /// <summary>
        ///     Trigger to start proccessing a current dialog file.
        /// </summary>
        public static readonly CommandInfo Start = new("Start", StartCommandValue);

        private static string FormatProcessSceneCommandValue(string command)
        {
            return $"{DialogModeInfo.MODE_NAME} {command}";
        }
    }
}