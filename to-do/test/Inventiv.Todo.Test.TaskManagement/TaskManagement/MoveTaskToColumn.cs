using Inventiv.Todo.Module.TaskManagement;
using NUnit.Framework;

namespace Inventiv.ToDo.Test.UnitTest.TaskManagement
{
    [TestFixture]
    public class MoveTaskToColumn : ToDoTestBase
    {
        [Test]
        public void GIVEN_there_exists_a_task__WHEN_user_moves_the_task_to_a_column__THEN_it_is_removed_from_its_old_column_and_listed_in_its_new_column()
        {
            var board = CreateBoard();
            var task = CreateTask(board: board);
            var toColumn = CreateColumn(board: board);

            BeginTest();

            task.Move(toColumn);

            Assert.AreEqual(task.Name, toColumn.GetTasks()[0].Name);
        }

        [Test]
        public void GIVEN_there_exists_a_task_and_a_column_in_a_different_board__WHEN_user_moves_the_task_to_the_column__THEN_system_gives_an_error()
        {
            var task = CreateTask();
            var targetColumn = CreateColumn();

            BeginTest();

            Assert.Throws<TaskManagementException.ColumnDoesNotBelongToBoard>(() => task.Move(targetColumn));
        }
    }
}
