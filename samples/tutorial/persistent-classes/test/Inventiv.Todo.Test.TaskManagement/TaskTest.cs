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
        public void CreateTask__creates_a_task_using_given_name()
        {
            var taskManager = Context.Get<TaskManager>();

            BeginTest();

            var actual = taskManager.CreateTask("Write Tests");

            Verify.ObjectIsPersisted(actual);
            Assert.AreEqual("Write Tests", actual.Name);
        }
    }
}