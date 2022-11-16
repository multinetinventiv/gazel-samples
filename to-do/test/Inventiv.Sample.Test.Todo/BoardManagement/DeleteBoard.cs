namespace Inventiv.Sample.Test.Todo.BoardManagement;

[TestFixture]
public class DeleteBoard : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_board_with_columns_and_task_cards__WHEN_user_deletes_the_board__THEN_columns_and_task_cards_should_be_deleted_as_well()
    {
        var board = CreateBoard();
        var column = CreateColumn(board: board, taskCardCount: 2);

        var taskCards = column.GetTaskCards();

        BeginTest();

        board.Delete();

        Verify.ObjectIsDeleted(board);
        Verify.ObjectIsDeleted(column);
        Verify.ObjectIsDeleted(taskCards[0]);
        Verify.ObjectIsDeleted(taskCards[1]);
    }
}
