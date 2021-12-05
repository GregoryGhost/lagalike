namespace PatrickStar.MVU.Tests
{
    using System;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class Tests
    {
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
            dataFlowManager.Model.TryGetValue(TEST_CHAT_ID, out var actualModel);
            Assert.AreEqual(actualModel, expectedModel);
        }
        
        [Test]
        public async Task TestMultithreadChangingModelAsync()
        {
            throw new NotImplementedException(
                "TODO: реализовать тест по проверки многопоточного изменения Model в dataFlowManager");
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
                ChatId = TEST_CHAT_ID
            };

            return testUpdate;
        }

        private const string TEST_CHAT_ID = "testChatId";
    }
}