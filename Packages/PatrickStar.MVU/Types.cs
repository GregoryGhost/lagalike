namespace PatrickStar.MVU
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// A view which representatate a available menu.
    /// </summary>
    /// <typeparam name="TCommandType">A available command types.</typeparam>
    public interface IView<TCommandType>
        where TCommandType : Enum
    {
        /// <summary>
        /// A menu with available elements.
        /// </summary>
        Menu<TCommandType> Menu { get; }

        /// <summary>
        /// Update a menu with actual model.
        /// </summary>
        /// <param name="sourceMenu">A source menu.</param>
        /// <returns>Returns a updated view.</returns>
        IView<TCommandType> Update(Menu<TCommandType> sourceMenu);
    }

    /// <summary>
    /// A menu element.
    /// </summary>
    public interface IElement
    {
    }

    /// <summary>
    /// A menu button.
    /// </summary>
    /// <typeparam name="TCommandType">Available command types.</typeparam>
    public record Button<TCommandType> : IElement
        where TCommandType : Enum
    {
        /// <summary>
        /// Available command types.
        /// </summary>
        public ICommand<TCommandType> Cmd { get; init; } = null!;

        /// <summary>
        /// A menu button label.
        /// </summary>
        public string Label { get; init; } = null!;
    }

    /// <summary>
    /// A menu message element.
    /// </summary>
    public record MessageElement : IElement
    {
        /// <summary>
        /// A message text.
        /// </summary>
        public string Text { get; init; } = null!;
    }

    /// <summary>
    /// A menu.
    /// </summary>
    /// <typeparam name="TCommandType"></typeparam>
    public record Menu<TCommandType> : IElement
        where TCommandType : Enum
    {
        /// <summary>
        /// Menu buttons.
        /// </summary>
        public Button<TCommandType>[][] Buttons { get; init; } = null!;

        /// <summary>
        /// A menu message.
        /// </summary>
        public MessageElement MessageElement { get; init; } = null!;
    }

    /// <summary>
    /// A new telegram update representation.
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// A chat id.
        /// </summary>
        string ChatId { get; init; }
    }

    /// <summary>
    /// A manager which control model update by triggered command and actualize view by actual model.
    /// </summary>
    /// <typeparam name="TModel">A model type.</typeparam>
    /// <typeparam name="TViewMapper">A view mapper type.</typeparam>
    /// <typeparam name="TUpdate">A model update type.</typeparam>
    /// <typeparam name="TCommandType">A command type.</typeparam>
    public interface IDataFlowManager<TModel, TViewMapper,
        TUpdate, TCommandType>
        where TModel : IModel, IEquatable<TModel>
        where TViewMapper : IViewMapper<TCommandType>
        where TCommandType : Enum
        where TUpdate : IUpdate
    {
        /// <summary>
        /// A cached model to update.
        /// </summary>
        IModelCache<TModel> Model { get; init; }

        /// <summary>
        /// A post proccessor which proccess a view after update.
        /// </summary>
        IPostProccessor<TCommandType, TUpdate> PostProccessor { get; init; }

        /// <summary>
        /// A model updater which update a model by a triggered command.
        /// </summary>
        IUpdater<TCommandType, TModel> Updater { get; init; }

        /// <summary>
        /// A view mapper which update view by actual model.
        /// </summary>
        TViewMapper ViewMapper { get; init; }
        
        /// <summary>
        /// An initial model.
        /// </summary>
        TModel InitialModel { get; init; }

        /// <summary>
        /// Get an input command from update.
        /// </summary>
        /// <param name="update">A new update for a model.</param>
        /// <returns></returns>
        ICommand<TCommandType> GetInputCommand(TUpdate update);

        /// <summary>
        /// Proccess a message update.
        /// </summary>
        /// <param name="update">A new update for a model.</param>
        async Task ProccessMessageAsync(TUpdate update)
        {
            var inputCommand = GetInputCommand(update);
            await ProccessCommandAsync(inputCommand, update.ChatId, update).ConfigureAwait(false);
        }

        /// <summary>
        /// Proccess a model and a view by triggered command. 
        /// </summary>
        /// <param name="command">A triggered command.</param>
        /// <param name="chatId">A chat id.</param>
        /// <param name="update">A new update for a model.</param>
        async Task ProccessCommandAsync(ICommand<TCommandType> command, string chatId, TUpdate update)
        {
            var (isInitialModelState, model) = GetModelByChatId(chatId);
            var (outputCommand, updatedModel) = await Updater.UpdateAsync(command, model).ConfigureAwait(false);

            var info = new UpdateViewInfo
            {
                ChatId = chatId,
                UpdateInfo = update,
                UpdatedModel = updatedModel,
                SourceModel = model,
                IsInitialModelState = isInitialModelState
            };
            await UpdateView(info).ConfigureAwait(false);

            if (outputCommand != null)
            {
                await ProccessCommandAsync(outputCommand, chatId, update).ConfigureAwait(false);
            }
        }

        private async Task UpdateView(UpdateViewInfo info)
        {
            var wasUpdatedModel = !info.UpdatedModel.Equals(info.SourceModel);
            var needToUpdateModel = wasUpdatedModel || info.IsInitialModelState;
            if (needToUpdateModel)
            {
                Model.Set(info.ChatId, info.UpdatedModel);

                var view = ViewMapper.Map(info.UpdatedModel);
                await PostProccessor.ProccessAsync(view, info.UpdateInfo).ConfigureAwait(false);
            }
        }

        private (bool isInitialModelState, TModel model) GetModelByChatId(string chatId)
        {
            var isInitialModelState = !Model.TryGetValue(chatId, out var model);
            
            return (isInitialModelState, model ?? InitialModel);
        }

        private struct UpdateViewInfo
        {
            public string ChatId { get; init; }
            public TUpdate UpdateInfo { get; init; }
            public TModel UpdatedModel { get; init; }
            public TModel SourceModel { get; init; }
            public bool IsInitialModelState { get; init; }
        }
    }
    
    /// <summary>
    /// A post proccessor which proccess a view after update.
    /// </summary>
    /// <typeparam name="TCommandType">An available command types.</typeparam>
    /// <typeparam name="TUpdate">A new update for a model.</typeparam>
    public interface IPostProccessor<TCommandType, in TUpdate>
        where TCommandType : Enum
        where TUpdate : IUpdate
    {
        /// <summary>
        /// Procces a view after update.
        /// </summary>
        /// <param name="view">A proccessed view.</param>
        /// <param name="update">A new update.</param>
        /// <returns>Returns nothing.</returns>
        Task ProccessAsync(IView<TCommandType> view, TUpdate update);
    }

    /// <summary>
    /// A view mapper which update view by actual model.
    /// </summary>
    /// <typeparam name="TCommandType">An available command types.</typeparam>
    public interface IViewMapper<TCommandType>
        where TCommandType : Enum
    {
        /// <summary>
        /// Map a actual model values for a view menu.
        /// </summary>
        /// <param name="model">A updated model.</param>
        /// <returns>Returns updated view by actual model.</returns>
        IView<TCommandType> Map(IModel model);
    }

    /// <inheritdoc />
    public abstract class BaseMainViewMapper<TCommandType, TModelType> : IViewMapper<TCommandType>
        where TCommandType : Enum
        where TModelType : Enum
    {
        private readonly IDictionary<TModelType, IViewMapper<TCommandType>> _views;

        /// <summary>
        /// Initialize dependencies.
        /// </summary>
        /// <param name="views">Available view by model type.</param>
        protected BaseMainViewMapper(IDictionary<TModelType, IViewMapper<TCommandType>> views)
        {
            _views = views;
        }

        /// <inheritdoc />
        public IView<TCommandType> Map(IModel model)
        {
            if (_views.TryGetValue((TModelType)model.Type, out var viewMapper))
                return viewMapper.Map(model);

            throw new KeyNotFoundException($"Can't find a key ({model}) in the dictionary of views.");
        }
    }

    /// <inheritdoc cref="PatrickStar.MVU.IView{TCommandType}" />
    public abstract record BaseMenuView<TCommandType> : IView<TCommandType>
        where TCommandType : Enum
    {
        /// <inheritdoc />
        public Menu<TCommandType> Menu { get; protected init; } = null!;

        /// <inheritdoc />
        public abstract IView<TCommandType> Update(Menu<TCommandType> sourceMenu);
    }

    /// <summary>
    /// A model updater which update a model by a triggered command.
    /// </summary>
    /// <typeparam name="TCommandType">An available command types.</typeparam>
    /// <typeparam name="TModel">An model type.</typeparam>
    public interface IUpdater<TCommandType, TModel>
        where TCommandType : Enum
        where TModel : IModel, IEquatable<TModel>
    {
        /// <summary>
        /// Update a model by a triggered command.
        /// </summary>
        /// <param name="command">A triggered command.</param>
        /// <param name="model">A updating model.</param>
        /// <returns>Returns an possible output command and an updated model.</returns>
        Task<(ICommand<TCommandType>? OutputCommand, TModel UpdatedModel)> UpdateAsync(ICommand<TCommandType> command,
            TModel model);
    }

    /// <summary>
    /// A model which representate any stored values.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// A model type.
        /// </summary>
        Enum Type { get; }
    }

    /// <summary>
    /// A base command to trigger update a model.
    /// </summary>
    /// <typeparam name="TCommandType">Available command types.</typeparam>
    public record BaseCommand<TCommandType> : ICommand<TCommandType>
        where TCommandType : Enum
    {
        /// <inheritdoc />
        public virtual TCommandType Type { get; init; } = default!;
    }

    /// <summary>
    /// A representation of a command to trigger.
    /// </summary>
    /// <typeparam name="TCommandType">Available command types.</typeparam>
    public interface ICommand<out TCommandType>
        where TCommandType : Enum
    {
        /// <summary>
        /// A command type.
        /// </summary>
        TCommandType Type { get; }
    }
}