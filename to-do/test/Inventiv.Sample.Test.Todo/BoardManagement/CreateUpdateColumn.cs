using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo.BoardManagement;

[TestFixture]
public class CreateUpdateColumn : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_board__WHEN_user_adds_a_new_column_to_it__THEN_the_board_lists_it_among_its_columns()
    {
        var board = CreateBoard();

        BeginTest();

        var column = board.AddColumn("column");

        Verify.ObjectIsPersisted(column);
        Assert.That(column.Name, Is.EqualTo("column"));

        var boardColumns = board.GetColumns();

        Assert.That(boardColumns.Count, Is.EqualTo(1));
        Assert.That(boardColumns[0], Is.EqualTo(column));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void GIVEN_there_exists_a_board__WHEN_user_creates_a_new_column_with_an_empty_name__THEN_system_gives_an_error(string name)
    {
        var board = CreateBoard();

        BeginTest();

        Assert.That(() => board.AddColumn(name), Throws.TypeOf<TodoExceptions.RequiredParameterIsMissing>());
    }

    [Test]
    public void GIVEN_there_exists_a_column__WHEN_user_updates_its_name__THEN_column_is_listed_with_its_new_name()
    {
        var column = CreateColumn("test");

        BeginTest();

        column.Update("newTest");

        Assert.That(column.Name, Is.EqualTo("newTest"));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void GIVEN_there_exists_a_column__WHEN_user_updates_the_column_with_an_empty_name__THEN_system_gives_an_error(string name)
    {
        var column = CreateColumn();

        BeginTest();

        Assert.That(() => column.Update(name), Throws.TypeOf<TodoExceptions.RequiredParameterIsMissing>());
    }

    [Test]
    public void GIVEN_there_exists_a_column_with_task_cards__WHEN_user_clears_the_column__THEN_all_task_cards_under_that_column_will_be_deleted()
    {
        var column = CreateColumn(taskCardCount: 2);
        var taskCards = column.GetTaskCards();

        BeginTest();

        column.Clear();

        Assert.That(column.GetTaskCards(), Is.Empty);
        Verify.ObjectIsDeleted(taskCards[0]);
        Verify.ObjectIsDeleted(taskCards[1]);
    }
}
