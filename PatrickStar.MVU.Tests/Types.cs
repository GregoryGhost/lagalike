namespace PatrickStar.MVU.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public record TestCmd : BaseCommand<CmdType>
    {
        public string TestProp { get; init; } = null!;

        /// <inheritdoc />
        public override CmdType Type => CmdType.Cmd1;
    }
    
    public record TestCmd2Repeated : BaseCommand<CmdType>
    {
        public string TestProp { get; init; } = null!;

        /// <inheritdoc />
        public override CmdType Type => CmdType.Cmd2;
    }
    
    public record TestCmd3Repeated : BaseCommand<CmdType>
    {
        public string TestProp { get; init; } = null!;

        /// <inheritdoc />
        public override CmdType Type => CmdType.Cmd3Repeated;
    }

    public class TestModelCache : IModelCache<Model1>
    {
        private readonly IDictionary<string, Model1> _cache;
        public TestModelCache()
        {
            _cache = new ConcurrentDictionary<string, Model1>();
        }
        /// <inheritdoc />
        public void Set(string chatId, Model1 value)
        {
            if (_cache.ContainsKey(chatId))
            {
                _cache[chatId] = value;
            }
            else
            {
                _cache.Add(chatId, value);
            }
        }

        /// <inheritdoc />
        public bool TryGetValue(string chatId, [MaybeNullWhen(false)]out Model1 value)
        {
            return _cache.TryGetValue(chatId, out value);
        }
    }

    public class DataFlowManager : IDataFlowManager<Model1, MainViewMapper, TestUpdate, CmdType>
    {
        public DataFlowManager(IModelCache<Model1> model, IPostProccessor<CmdType, TestUpdate> postProccessor, IUpdater<CmdType, Model1> updater, MainViewMapper viewMapper)
        {
            Model = model;
            PostProccessor = postProccessor;
            Updater = updater;
            ViewMapper = viewMapper;
            InitialModel = new Model1
            {
                Test = false
            };
        }

        /// <inheritdoc />
        public IModelCache<Model1> Model { get; init; }

        /// <inheritdoc />
        public IPostProccessor<CmdType, TestUpdate> PostProccessor { get; init; }

        /// <inheritdoc />
        public IUpdater<CmdType, Model1> Updater { get; init; }

        /// <inheritdoc />
        public MainViewMapper ViewMapper { get; init; }

        /// <inheritdoc />
        public Model1 InitialModel { get; init; }

        /// <inheritdoc />
        public ICommand<CmdType> GetInputCommand(TestUpdate update)
        {
            var dictCommands = new Dictionary<CmdType, ICommand<CmdType>>
            {
                { CmdType.Cmd1, new TestCmd() },
                { CmdType.Cmd2, new TestCmd2Repeated() }
            };
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

    public class PostProccessor : IPostProccessor<CmdType, TestUpdate>
    {
        /// <inheritdoc />
        public async Task ProccessAsync(IView<CmdType> view, TestUpdate info)
        {
            await Task.CompletedTask;
        }
    }

    public class TestUpdater : IUpdater<CmdType, Model1>
    {
        /// <inheritdoc />
        public async Task<(ICommand<CmdType>? OutputCommand, Model1 UpdatedModel)> UpdateAsync(ICommand<CmdType> command,
            Model1 model)
        {
            var outputCmd = command.Type switch
            {
                CmdType.Cmd1 => null,
                CmdType.Cmd2 => new TestCmd3Repeated(),
                CmdType.Cmd3Repeated => null,
                _ => throw new ArgumentOutOfRangeException($"Unknown {nameof(command)}: {command}") 
            };
            var updatedModel = model with
            {
               Test = true,
               GotTestCmd2Repeated = command.Type is CmdType.Cmd3Repeated
            };

            return (outputCmd, updatedModel);
        }
    }

    public record TestUpdate : IUpdate
    {
        public string Data { get; init; } = null!;

        /// <inheritdoc />
        public string ChatId { get; init; } = null!;
    }

    public enum ModelType
    {
        Model1,

        Model2
    }

    public enum CmdType
    {
        Cmd1,

        Cmd2,
        
        Cmd3Repeated
    }

    public record Model1 : IModel
    {
        public bool Test { get; init; }
        
        public bool GotTestCmd2Repeated { get; init; }
        
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
                   .Button("test2", (ICommand<TCommandType>)new TestCmd2Repeated())
                   .Build("test msg");
        }

        /// <inheritdoc />
        public override IView<TCommandType> Update(Menu<TCommandType> sourceMenu)
        {
            throw new NotImplementedException();
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