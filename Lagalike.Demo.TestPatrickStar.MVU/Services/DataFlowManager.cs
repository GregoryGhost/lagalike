namespace Lagalike.Demo.TestPatrickStar.MVU.Services
{
    using System;
    using System.Collections.Generic;

    using Lagalike.Demo.TestPatrickStar.MVU.Commands;
    using Lagalike.Demo.TestPatrickStar.MVU.Models;
    using Lagalike.Demo.TestPatrickStar.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;

    using Newtonsoft.Json;

    using PatrickStar.MVU;

    public class DataFlowManager : IDataFlowManager<Model, ViewMapper, TelegramUpdate, CommandTypes>
    {
        private readonly CommandsFactory _commandsFactory;

        private readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> _commands;

        public DataFlowManager(TestPatrickStarCache model, TestPatrickPostProccessor postProccessor, TestPatrickUpdater updater,
            ViewMapper viewMapper, CommandsFactory commandsFactory)
        {
            _commandsFactory = commandsFactory;
            Model = model;
            PostProccessor = postProccessor;
            Updater = updater;
            ViewMapper = viewMapper;
            InitialModel = new Model
            {
                CurrentNumber = 0
            };
            _commands = commandsFactory.GetCommands();
        }

        /// <inheritdoc />
        public Model InitialModel { get; init; }

        /// <inheritdoc />
        public ICommand<CommandTypes> GetInputCommand(TelegramUpdate update)
        {
            var commandType = update.RequestType switch
            {
                RequestTypes.CallbackData => JsonConvert.DeserializeObject<BaseCommand<CommandTypes>>(update.Update.CallbackQuery.Data),
                RequestTypes.Message or RequestTypes.EditedMessage => _commandsFactory.GetMenuCommand(),
                _ => throw new ArgumentOutOfRangeException("Unknown request type")
            };
            if (commandType?.Type == null)
                throw new NullReferenceException($"Command type is null, the source update data: {update.Update.CallbackQuery.Data}");

            if (_commands.ContainsKey(commandType.Type))
                return commandType;

            throw new KeyNotFoundException($"Not found the command type {commandType.Type}");
        }

        /// <inheritdoc />
        public IModelCache<Model> Model { get; init; }

        /// <inheritdoc />
        public IPostProccessor<CommandTypes, TelegramUpdate> PostProccessor { get; init; }

        /// <inheritdoc />
        public IUpdater<CommandTypes> Updater { get; init; }

        /// <inheritdoc />
        public ViewMapper ViewMapper { get; init; }
    }
}