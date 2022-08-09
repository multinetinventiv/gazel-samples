using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement.BoardManagement
{
    [TestFixture]
    public class DeleteBoard : ToDoTestBase
    {
        [Test]
        public void GIVEN_there_exists_a_board_with_columns_and_tasks__WHEN_user_deletes_the_board__THEN_columns_and_tasks_should_be_deleted_as_well()
        {
            var board = CreateBoard();
            var column = CreateColumn(board: board, taskCount: 2);

            var tasks = column.GetTasks();

            BeginTest();

            board.Delete();

            Verify.ObjectIsDeleted(board);
            Verify.ObjectIsDeleted(column);
            Verify.ObjectIsDeleted(tasks[0]);
            Verify.ObjectIsDeleted(tasks[1]);
        }
    }
}
