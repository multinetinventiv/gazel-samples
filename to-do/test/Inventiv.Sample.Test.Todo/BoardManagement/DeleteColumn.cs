namespace Inventiv.Sample.Test.Todo.BoardManagement;

[TestFixture]
public class DeleteColumn : ToDoTestBase
{
    [Test]
    public void Given_there_exists_a_column_with_task_cards__When_user_deletes_the_column__Then_all_task_cards_under_that_column_will_be_deleted_along_with_the_column_itself()
    {
        var column = CreateColumn(taskCardCount: 2);

        var taskCards = column.GetTaskCards();

        BeginTest();

        column.Delete();

        Verify.ObjectIsDeleted(column);
        Verify.ObjectIsDeleted(taskCards[0]);
        Verify.ObjectIsDeleted(taskCards[1]);
    }

    [Test]
    public void Given_there_exists_a_board_with_columns_and_task_cards__When_user_deletes_one_column_by_move_and_delete__Then_task_cards_will_be_moved_to_other_column_before_deletion_of_selected_column()
    {
        var fromColumn = CreateColumn(taskCardCount: 2);
        var toColumn = CreateColumn(board: fromColumn.Board);

        var taskCards = fromColumn.GetTaskCards();

        BeginTest();

        fromColumn.MoveTaskCardsAndDelete(toColumn);

        Verify.ObjectIsDeleted(fromColumn);

        Assert.That(fromColumn.GetTaskCards().Count, Is.EqualTo(0));
        Assert.That(toColumn.GetTaskCards().Count, Is.EqualTo(2));

        Assert.That(taskCards[0].Column, Is.EqualTo(toColumn));
        Assert.That(taskCards[1].Column, Is.EqualTo(toColumn));
    }
}
