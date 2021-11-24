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
            Assert.AreEqual(dataFlowManager.Model, expectedModel);
        }

        [Test]
        public async Task TestOutputCommandProccessingAsync()
        {
            throw new NotImplementedException(
                "TODO: реализовать обработку outputCommand в ProccessMessageAsync у dataFlowManager");
        }
        
        [Test]
        public async Task TestMultithreadChangingModelAsync()
        {
            throw new NotImplementedException(
                "TODO: реализовать тест по проверки многопоточного изменения Model в dataFlowManager");
        }

        private static IDataFlowManager<IModel, MainViewMapper, TestUpdate, CmdType> GetDataFlowManager()
        {
            var initialModel = new Model1
            {
                Test = false
            };
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
                Data = JsonConvert.SerializeObject(cmd)
            };

            return testUpdate;
        }
    }
}