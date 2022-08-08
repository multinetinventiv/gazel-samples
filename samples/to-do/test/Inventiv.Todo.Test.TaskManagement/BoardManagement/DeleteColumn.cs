using NUnit.Framework;

namespace Inventiv.ToDo.Test.UnitTest.TaskManagement.BoardManagement
{
    [TestFixture]
    public class DeleteColumn : ToDoTestBase
    {
        [Test]
        public void Given_there_exists_a_column_with_tasks__When_user_deletes_the_column__Then_all_tasks_under_that_column_will_be_deleted_along_with_the_column_itself()
        {
            var column = CreateColumn(taskCount: 2);

            var tasks = column.GetTasks();

            BeginTest();

            column.Delete();

            Verify.ObjectIsDeleted(column);
            Verify.ObjectIsDeleted(tasks[0]);
            Verify.ObjectIsDeleted(tasks[1]);
        }

        [Test]
        public void Given_there_exists_a_board_with_columns_and_tasks__When_user_deletes_one_column_by_move_and_delete__Then_tasks_will_be_moved_to_other_column_before_deletion_of_selected_column()
        {
            var fromColumn = CreateColumn(taskCount: 2);
            var toColumn = CreateColumn(board: fromColumn.Board);

            var tasks = fromColumn.GetTasks();

            BeginTest();

            fromColumn.MoveTasksAndDelete(toColumn);

            Verify.ObjectIsDeleted(fromColumn);

            Assert.AreEqual(0, fromColumn.GetTasks().Count);
            Assert.AreEqual(2, toColumn.GetTasks().Count);

            Assert.AreEqual(toColumn, tasks[0].Column);
            Assert.AreEqual(toColumn, tasks[1].Column);
        }
    }
}
