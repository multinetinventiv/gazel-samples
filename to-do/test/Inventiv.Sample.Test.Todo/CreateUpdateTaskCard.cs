using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class CreateUpdateTaskCard : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_column__WHEN_user_adds_a_task_card_to_it__THEN_the_column_lists_it_among_its_task_cards()
    {
        var column = CreateColumn();

        BeginTest();

        var taskCard = column.AddTaskCard("task");

        Verify.ObjectIsPersisted(taskCard);
        Assert.That(taskCard.Name, Is.EqualTo("task"));

        var taskCards = column.GetTaskCards();
        Assert.That(taskCards.Count, Is.EqualTo(1));
        Assert.That(taskCards[0], Is.EqualTo(taskCard));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void GIVEN_there_exists_a_column__WHEN_user_adds_a_task_card_with_an_empty_name__THEN_system_gives_an_error(string name)
    {
        var column = CreateColumn();

        BeginTest();

        Assert.That(() => column.AddTaskCard(name), Throws.TypeOf<TodoExceptions.RequiredParameterIsMissing>());
    }

    [Test]
    public void GIVEN_there_exists_a_task_card__WHEN_user_updates_name_and_notes_of_the_task_card__THEN_task_will_show_its_new_name_and_notes()
    {
        var taskCard = CreateTaskCard();

        BeginTest();

        taskCard.Update("test", "notes");

        Assert.That(taskCard.Name, Is.EqualTo("test"));
        Assert.That(taskCard.Notes, Is.EqualTo("notes"));
    }

    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase(" ", " ")]
    public void GIVEN_there_exists_a_task_card__WHEN_user_updates_the_task_card_with_an_empty_name_or_notes__THEN_task_card_will_not_be_updated(string name, string notes)
    {
        var taskCard = CreateTaskCard(taskCardName: "task", notes: "notes");

        BeginTest();

        Assert.That(() => taskCard.Update(name, notes), Throws.Nothing);

        Assert.That(taskCard.Name, Is.EqualTo("task"));
        Assert.That(taskCard.Notes, Is.EqualTo("notes"));
    }
}
