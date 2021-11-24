namespace Lagalike.Telegram.Shared.Contracts.PatrickStar.MVU
{
    using System;
    using System.Threading.Tasks;

    using global::PatrickStar.MVU;

    using global::Telegram.Bot.Types;
    using global::Telegram.Bot.Types.Enums;

    public abstract class BaseMvuUpdateHandlerService<TModel, TViewMapper, TCommandType> : ITelegramUpdateHandler
        where TModel : IModel
        where TViewMapper : IViewMapper<TCommandType>
        where TCommandType : Enum
    {
        private readonly IDataFlowManager<TModel, TViewMapper, TelegramUpdate, TCommandType> _dataFlowManager;

        protected BaseMvuUpdateHandlerService(IDataFlowManager<TModel, TViewMapper, TelegramUpdate, TCommandType>  dataFlowManager)
        {
            _dataFlowManager = dataFlowManager;
        }
        
        /// <inheritdoc />
        public async Task HandleUpdateAsync(Update update)
        {
            if (update.Type is UpdateType.CallbackQuery)
            {
                var callbackUpdate = new TelegramCallbackDataUpdate
                {
                    ChatId = update.CallbackQuery.From.Id.ToString(),
                    Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(callbackUpdate);
            }
            else if (update.Type is UpdateType.Message)
            {
                var messageUpdate = new TelegramMsgUpdate
                {
                    ChatId = update.Message.From.Id.ToString(),
                    Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(messageUpdate);
            }
            else if (update.Type is UpdateType.EditedMessage)
            {
                var editedMsgUpdate = new TelegramEditedMsgUpdate
                {
                    ChatId = update.EditedMessage.From.Id.ToString(),
                    Update = update,
                };
                await _dataFlowManager.ProccessMessageAsync(editedMsgUpdate);
            }
            else
            {
                //Don't proccess other types of update because it doesn't matter
            }
        }
    }
}