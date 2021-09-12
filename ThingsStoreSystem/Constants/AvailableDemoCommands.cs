namespace ThingsStore.Constants
{
    using Lagalike.Telegram.Shared;

    using ThingsStore.Services;

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
        ///     Trigger to go shopping.
        /// </summary>
        public static readonly CommandInfo GoShopping = new("Go shopping", FormatProcessSceneCommandValue("go shopping"));

        /// <summary>
        ///     Trigger to buy an item.
        /// </summary>
        public static readonly CommandInfo BuyItem = new("Buy", FormatProcessSceneCommandValue("buy item"));

        public static readonly CommandInfo ShowOrderHistory = new(
            "Show order history",
            FormatProcessSceneCommandValue("show order history"));

        public static readonly CommandInfo GetNextItems = new("Next", FormatProcessSceneCommandValue("get next items"));

        public static readonly CommandInfo GetPreviousItems = new(
            "Previous",
            FormatProcessSceneCommandValue("get previous items"));

        public static readonly CommandInfo GetStartItems = new("<<", FormatProcessSceneCommandValue("get start items"));

        public static readonly CommandInfo GetEndItems = new(">>", FormatProcessSceneCommandValue("get end items"));

        public static readonly CommandInfo SelectItem = new("Select", FormatProcessSceneCommandValue("select item to buy"));

        private static string FormatProcessSceneCommandValue(string command)
        {
            return $"{ThingsStoreModeInfo.MODE_NAME} {command}";
        }
    }
}