namespace PatrickStar.MVU
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IView<TCommandType>
        where TCommandType : Enum
    {
        Menu<TCommandType> Menu { get; }

        IView<TCommandType> Update(Menu<TCommandType> sourceMenu);
    }

    public interface IElement
    {
        //TODO: хз что тут должно быть, но пусть пока будет интерфейсом   
    }

    public record Button<TCommandType> : IElement
        where TCommandType : Enum
    {
        public ICommand<TCommandType> Cmd { get; init; } = null!;

        public string Label { get; init; } = null!;
    }

    public record MessageElement : IElement
    {
        public string Text { get; init; } = null!;
    }

    public record Menu<TCommandType> : IElement
        where TCommandType : Enum
    {
        public Button<TCommandType>[][] Buttons { get; init; } = null!;

        public MessageElement MessageElement { get; init; } = null!;
    }

    public interface IUpdate
    {
        string ChatId { get; init; }
    }

    public interface IDataFlowManager<TModel, TViewMapper,
        TUpdate, TCommandType>
        where TModel : IModel
        where TViewMapper : IViewMapper<TCommandType>
        where TCommandType : Enum
        where TUpdate : IUpdate
    {
        IModelCache<TModel> Model { get; init; }

        IPostProccessor<TCommandType, TUpdate> PostProccessor { get; init; }

        IUpdater<TCommandType> Updater { get; init; }

        TViewMapper ViewMapper { get; init; }
        
        TModel InitialModel { get; init; }

        ICommand<TCommandType> GetInputCommand(TUpdate update);

        async Task ProccessMessageAsync(TUpdate update)
        {
            var inputCommand = GetInputCommand(update);
            await ProccessCommandAsync(inputCommand, update.ChatId, update).ConfigureAwait(false);
        }

        async Task ProccessCommandAsync(ICommand<TCommandType> command, string chatId, TUpdate update)
        {
            if (!Model.TryGetValue(chatId, out var model))
            {
                model = InitialModel;
            }
            var (outputCommand, updatedModel) = await Updater.UpdateAsync(command, model).ConfigureAwait(false);
            Model.Set(chatId, (TModel)updatedModel);
            var view = ViewMapper.Map(updatedModel);
            await PostProccessor.ProccessAsync(view, update).ConfigureAwait(false);
            
            if (outputCommand != null)
            {
                await ProccessCommandAsync(outputCommand, chatId, update).ConfigureAwait(false);
            }
        }
    }

    public interface IPostProccessor<TCommandType, in TUpdate>
        where TCommandType : Enum
        where TUpdate : IUpdate
    {
        Task ProccessAsync(IView<TCommandType> view, TUpdate update);
    }

    public interface IViewMapper<TCommandType>
        where TCommandType : Enum
    {
        IView<TCommandType> Map(IModel model);
    }

    public abstract class BaseMainViewMapper<TCommandType, TModelType> : IViewMapper<TCommandType>
        where TCommandType : Enum
        where TModelType : Enum
    {
        private readonly IDictionary<TModelType, IViewMapper<TCommandType>> _views;

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

    public abstract record BaseMenuView<TCommandType> : IView<TCommandType>
        where TCommandType : Enum
    {
        /// <inheritdoc />
        public Menu<TCommandType> Menu { get; protected init; } = null!;

        /// <inheritdoc />
        public abstract IView<TCommandType> Update(Menu<TCommandType> sourceMenu);
    }

    public interface IUpdater<TCommandType>
        where TCommandType : Enum
    {
        Task<(ICommand<TCommandType>? OutputCommand, IModel UpdatedModel)> UpdateAsync(ICommand<TCommandType> command,
            IModel model);
    }

    public interface IModel
    {
        Enum Type { get; }
    }

    public record BaseCommand<TCommandType> : ICommand<TCommandType>
        where TCommandType : Enum
    {
        /// <inheritdoc />
        public virtual TCommandType Type { get; init; } = default!;
    }

    public interface ICommand<out TCommandType>
        where TCommandType : Enum
    {
        TCommandType Type { get; }
    }
}