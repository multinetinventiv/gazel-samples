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
        Assert.That(actual.Name, Is.EqualTo("Write Tests"));
    }

    [Test]
    public void Complete__marks_task_card_as_completed()
    {
        var todoManager = Context.Get<TodoManager>();
        var taskCard = todoManager.CreateTaskCard("Write Tests");

        BeginTest();

        taskCard.Complete();

        Assert.That(taskCard.Completed, Is.True);
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

        Assert.That(actual.Count, Is.EqualTo(2));
        Assert.That(actual[0].Name, Is.EqualTo("completed 1"));
        Assert.That(actual[1].Name, Is.EqualTo("completed 2"));
    }
}
