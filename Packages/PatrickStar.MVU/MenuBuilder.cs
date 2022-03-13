namespace PatrickStar.MVU
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A menu builder.
    /// </summary>
    /// <typeparam name="TCommandType">A type of available commands.</typeparam>
    public class MenuBuilder<TCommandType>
        where TCommandType : Enum
    {
        private readonly List<Button<TCommandType>[]> _buttons = new();

        private readonly List<Button<TCommandType>> _row = new();

        /// <summary>
        /// Build a menu.
        /// </summary>
        /// <param name="messageText">A message text in a menu.</param>
        /// <returns>Returns a built menu.</returns>
        public Menu<TCommandType> Build(string messageText)
        {
            var message = new MessageElement
            {
                Text = messageText
            };
            Row();
            var menu = new Menu<TCommandType>
            {
                MessageElement = message,
                Buttons = _buttons.ToArray()
            };

            return menu;
        }

        /// <summary>
        /// Add a button to a menu.
        /// </summary>
        /// <param name="label">A button label.</param>
        /// <param name="cmd">A used command on a button click.</param>
        /// <returns>Returns a building menu.</returns>
        public MenuBuilder<TCommandType> Button(string label, ICommand<TCommandType> cmd)
        {
            var btn = new Button<TCommandType>
            {
                Cmd = cmd,
                Label = label
            };
            _row.Add(btn);

            return this;
        }

        /// <summary>
        /// Add a row to a menu.
        /// </summary>
        /// <returns>Returns a building menu.</returns>
        public MenuBuilder<TCommandType> Row()
        {
            if (!_row.Any())
            {
                _row.Clear();
                return this;
            }
            _buttons.Add(_row.ToArray());
            _row.Clear();

            return this;
        }
    }
}