namespace Lagalike.Demo.MVU.Services
{
    using System;
    using System.Collections.Generic;

    using Lagalike.Demo.MVU.Commands;
    using Lagalike.Demo.MVU.Models;
    using Lagalike.Demo.MVU.Services.Views;
    using Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU;

    using Newtonsoft.Json;

    using PatrickStar.MVU;

    /// <summary>
    /// The demo data flow manager which controls Telegram and 
    /// </summary>
    public class DataFlowManager : IDataFlowManager<Model, ViewMapper, TelegramUpdate, CommandTypes>
    {
        private readonly CommandsFactory _commandsFactory;

        private readonly IReadOnlyDictionary<CommandTypes, ICommand<CommandTypes>> _commands;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="model">The demo model.</param>
        /// <param name="postProccessor">The Telegram update post processor.</param>
        /// <param name="updater">The Telegram update handler.</param>
        /// <param name="viewMapper">The view mapper.</param>
        /// <param name="commandsFactory">The demo commands factory.</param>
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
                RequestTypes.Message or RequestTypes.EditedMessage => _commandsFactory.MenuCommand,
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
        public IUpdater<CommandTypes, Model> Updater { get; init; }

        /// <inheritdoc />
        public ViewMapper ViewMapper { get; init; }
    }
}