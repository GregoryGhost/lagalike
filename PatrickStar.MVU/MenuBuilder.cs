namespace PatrickStar.MVU
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MenuBuilder<TCommandType>
        where TCommandType : Enum
    {
        private readonly List<Button<TCommandType>[]> _buttons = new();

        private readonly List<Button<TCommandType>> _row = new();

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