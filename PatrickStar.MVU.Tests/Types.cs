namespace PatrickStar.MVU.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public record TestCmd : BaseCommand<CmdType>
    {
        public string TestProp { get; init; } = null!;

        /// <inheritdoc />
        public override CmdType Type => CmdType.Cmd1;
    }
    
    public record TestCmd2 : BaseCommand<CmdType>
    {
        public string TestProp { get; init; } = null!;

        /// <inheritdoc />
        public override CmdType Type => CmdType.Cmd2;
    }

    public class DataFlowManager : IDataFlowManager<IModel, MainViewMapper, TestUpdate, CmdType>
    {
        public DataFlowManager(IModel model, IPostProccessor<CmdType> postProccessor, IUpdater<CmdType> updater, MainViewMapper viewMapper)
        {
            Model = model;
            PostProccessor = postProccessor;
            Updater = updater;
            ViewMapper = viewMapper;
        }

        /// <inheritdoc />
        public IModel Model { get; set; }

        /// <inheritdoc />
        public IPostProccessor<CmdType> PostProccessor { get; init; }

        /// <inheritdoc />
        public IUpdater<CmdType> Updater { get; init; }

        /// <inheritdoc />
        public MainViewMapper ViewMapper { get; init; }

        /// <inheritdoc />
        public ICommand<CmdType> GetInputCommand(TestUpdate update)
        {
            var dictCommands = new Dictionary<CmdType, ICommand<CmdType>>
            {
                { CmdType.Cmd1, new TestCmd() },
                { CmdType.Cmd2, new TestCmd2() }
            };      //TODO: скорее всего можно сделать через Type и через рефлексию сериализовать в нужный тип, возвращая при этом по ICommand интерфейсу
            var commandType = JsonConvert.DeserializeObject<BaseCommand<CmdType>>(update.Data);
            if (commandType?.Type == null)
            {
                throw new NullReferenceException($"Command type is null, the source update data: {update.Data}");
            }
            if (dictCommands.ContainsKey(commandType.Type))
            {
                return commandType;
            }
            
            throw new KeyNotFoundException($"Not found the command type {commandType.Type}");
        }
    }

    public class PostProccessor : IPostProccessor<CmdType>
    {
        /// <inheritdoc />
        public async Task ProccessAsync(IView<CmdType> view, Tinfo info)
        {
            await Task.CompletedTask;
        }
    }

    public class TestUpdater : IUpdater<CmdType>
    {
        /// <inheritdoc />
        public async Task<(ICommand<CmdType>? OutputCommand, IModel UpdatedModel)> UpdateAsync(ICommand<CmdType> command,
            IModel model)
        {
            var castedModel = model switch
            {
                Model1 m1 => m1,
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(model)} type: {model}")
            };
            var outputCmd = command.Type switch
            {
                CmdType.Cmd1 => null,
                CmdType.Cmd2 => new TestCmd(),
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}") 
            };
            var updatedModel = castedModel with
            {
               Test = true
            };

            return (outputCmd, updatedModel);
        }
    }

    public record TestUpdate
    {
        public string Data { get; init; } = null!;
    }

    public enum ModelType
    {
        Model1,

        Model2
    }

    public enum CmdType
    {
        Cmd1,

        Cmd2
    }

    public record Model1 : IModel
    {
        public bool Test { get; init; }
        
        public virtual Enum Type => ModelType.Model1;
    }

    public record MenuView1<TCommandType> : BaseMenuView<TCommandType>
        where TCommandType : Enum
    {
        public MenuView1()
        {
            Menu = new MenuBuilder<TCommandType>()
                   .Row()
                   .Button("test", (ICommand<TCommandType>)new TestCmd())
                   .Row()
                   .Button("test2", (ICommand<TCommandType>)new TestCmd2())
                   .Build("test msg");
        }
    }

    public class MainViewMapper : BaseMainViewMapper<CmdType, ModelType>
    {
        private static readonly Dictionary<ModelType, IViewMapper<CmdType>> Views = new()
        {
            { ModelType.Model1, new ViewMapper1<CmdType>() }
        };

        /// <inheritdoc />
        public MainViewMapper()
            : base(Views)
        {
        }
    }

    public class ViewMapper1<TCommandType> : IViewMapper<TCommandType>
        where TCommandType : Enum
    {
        private readonly IView<TCommandType> _view = new MenuView1<TCommandType>();

        public IView<TCommandType> Map(IModel model)
        {
            return _view;
        }
    }
}