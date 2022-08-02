using Gazel;
using Gazel.UnitTesting;
using Inventiv.Todo.Module.TaskManagement;
using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement
{
    [TestFixture]
    public class TaskTest : TestBase
    {
        [Test]
        public void CompleteTask__marks_task_as_completed()
        {
            var taskManager = Context.Get<TaskManager>();
            var task = taskManager.CreateTask("Write Tests");

            BeginTest();

            task.Complete();

            Assert.IsTrue(task.Completed);
        }
    }
}