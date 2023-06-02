namespace Inventiv.Sample.Test.Todo.BoardManagement;

[TestFixture]
public class ClearColumn : ToDoTestBase
{
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
