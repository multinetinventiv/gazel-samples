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
}
