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
        public void SayHello__says_hello()
        {
            var taskManager = Context.Get<TaskManager>();

            Assert.AreEqual("Hello World", taskManager.SayHello());
        }
    }
}