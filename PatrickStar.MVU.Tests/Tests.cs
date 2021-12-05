namespace PatrickStar.MVU.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class Tests
    {
        private const string TEST_CHAT_ID = "testChatId";        
        private const string TEST_CHAT_ID2 = "testChatId2";

        [Test]
        public async Task TestCommandProccessAsync()
        {
            var dataFlowManager = GetDataFlowManager();
            var update = GetTestUpdate();
            await dataFlowManager.ProccessMessageAsync(update);

            var expectedModel = new Model1
            {
                Test = true
            };
            dataFlowManager.Model.TryGetValue(TEST_CHAT_ID, out var actualModel);
            Assert.AreEqual(actualModel, expectedModel);
        }

        [Test]
        public async Task TestMultithreadChangingEachModelAsync()
        {
            var dataFlowManager = GetDataFlowManager();
            var update1 = GetTestUpdate();
            var update2 = GetTestUpdateOutputCmd();
            var updates = new[] { update1, update2 }.Select(dataFlowManager.ProccessMessageAsync);
            await Task.WhenAll(updates);
            
            var expectedModel1 = new Model1
            {
                Test = true,
                GotTestCmd2Repeated = false
            };            
            var expectedModel2 = new Model1
            {
                Test = true,
                GotTestCmd2Repeated = true
            };
            dataFlowManager.Model.TryGetValue(TEST_CHAT_ID, out var actualModel1);
            Assert.AreEqual(actualModel1, expectedModel1);
            
            dataFlowManager.Model.TryGetValue(TEST_CHAT_ID2, out var actualModel2);
            Assert.AreEqual(actualModel2, expectedModel2);
        }

        [Test]
        public async Task TestOutputCommandProccessingAsync()
        {
            var dataFlowManager = GetDataFlowManager();
            var update = GetTestUpdateOutputCmd();
            await dataFlowManager.ProccessMessageAsync(update);

            var expectedModel = new Model1
            {
                Test = true,
                GotTestCmd2Repeated = true
            };
            dataFlowManager.Model.TryGetValue(TEST_CHAT_ID2, out var actualModel);
            Assert.AreEqual(actualModel, expectedModel);
        }

        private static IDataFlowManager<Model1, MainViewMapper, TestUpdate, CmdType> GetDataFlowManager()
        {
            var initialModel = new TestModelCache();
            var proccessor = new PostProccessor();
            var updater = new TestUpdater();
            var viewManager = new MainViewMapper();
            var dataFlowManager = new DataFlowManager(initialModel, proccessor, updater, viewManager);

            return dataFlowManager;
        }

        private static TestUpdate GetTestUpdate()
        {
            var cmd = new TestCmd
            {
                TestProp = "kekw"
            };
            var testUpdate = new TestUpdate
            {
                Data = JsonConvert.SerializeObject(cmd),
                ChatId = TEST_CHAT_ID
            };

            return testUpdate;
        }

        private static TestUpdate GetTestUpdateOutputCmd()
        {
            var cmd = new TestCmd2Repeated
            {
                TestProp = "kekw"
            };
            var testUpdate = new TestUpdate
            {
                Data = JsonConvert.SerializeObject(cmd),
                ChatId = TEST_CHAT_ID2
            };

            return testUpdate;
        }
    }
}