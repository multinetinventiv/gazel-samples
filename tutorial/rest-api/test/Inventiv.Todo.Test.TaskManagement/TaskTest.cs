using Gazel;
using Gazel.UnitTesting;
using Inventiv.Todo.Module.TaskManagement;
using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement
{
    [TestFixture]
    public class TaskTest : TestBase
    {
        static TaskTest()
        {
            Config.RootNamespace = "Inventiv";
        }
        
        [Test]
        public void Tasks_ByCompleted__filters_tasks_by_completed_column()
        {
            var taskManager = Context.Get<TaskManager>();

            taskManager.CreateTask("incomplete");
            taskManager.CreateTask("completed 1").Complete();
            taskManager.CreateTask("completed 2").Complete();

            BeginTest();

            var actual = Context.Query<Tasks>().ByCompleted(true);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("completed 1", actual[0].Name);
            Assert.AreEqual("completed 2", actual[1].Name);
        }
    }
}