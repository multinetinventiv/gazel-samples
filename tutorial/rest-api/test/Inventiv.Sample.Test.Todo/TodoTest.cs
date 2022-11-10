using Gazel;
using Gazel.UnitTesting;
using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class TodoTest : TestBase
{
    static TodoTest()
    {
        Config.RootNamespace = "Inventiv";
    }

    [Test]
    public void CreateTaskCard__creates_a_task_card_using_given_name()
    {
        var todoManager = Context.Get<TodoManager>();

        BeginTest();

        var actual = todoManager.CreateTaskCard("Write Tests");

        Verify.ObjectIsPersisted(actual);
        Assert.AreEqual("Write Tests", actual.Name);
    }

    [Test]
    public void Complete__marks_task_card_as_completed()
    {
        var todoManager = Context.Get<TodoManager>();
        var taskCard = todoManager.CreateTaskCard("Write Tests");

        BeginTest();

        taskCard.Complete();

        Assert.IsTrue(taskCard.Completed);
    }

    [Test]
    public void TaskCards_ByCompleted__filters_tasks_by_completed_column()
    {
        var todoManager = Context.Get<TodoManager>();

        todoManager.CreateTaskCard("incomplete");
        todoManager.CreateTaskCard("completed 1").Complete();
        todoManager.CreateTaskCard("completed 2").Complete();

        BeginTest();

        var actual = Context.Query<TaskCards>().ByCompleted(true);

        Assert.AreEqual(2, actual.Count);
        Assert.AreEqual("completed 1", actual[0].Name);
        Assert.AreEqual("completed 2", actual[1].Name);
    }
}
