using Inventiv.Todo.Module.TaskManagement;
using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement.TaskManagement.BoardManagement
{
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
            Assert.AreEqual("column", column.Name);

            var boardColumns = board.GetColumns();

            Assert.AreEqual(1, boardColumns.Count);
            Assert.AreEqual(column, boardColumns[0]);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GIVEN_there_exists_a_board__WHEN_user_creates_a_new_column_with_an_empty_name__THEN_system_gives_an_error(string name)
        {
            var board = CreateBoard();

            BeginTest();

            Assert.Throws<TaskManagementException.RequiredParameterIsMissing>(() =>
            {
                board.AddColumn(name);
            });
        }

        [Test]
        public void GIVEN_there_exists_a_column__WHEN_user_updates_its_name__THEN_column_is_listed_with_its_new_name()
        {
            var column = CreateColumn("test");

            BeginTest();

            column.Update("newTest");

            Assert.AreEqual("newTest", column.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GIVEN_there_exists_a_column__WHEN_user_updates_the_column_with_an_empty_name__THEN_system_gives_an_error(string name)
        {
            var column = CreateColumn();

            BeginTest();

            Assert.Throws<TaskManagementException.RequiredParameterIsMissing>(() =>
            {
                column.Update(name);
            });
        }

        [Test]
        public void GIVEN_there_exists_a_column_with_tasks__WHEN_user_clears_the_column__THEN_all_tasks_under_that_column_will_be_deleted()
        {
            var column = CreateColumn(taskCount: 2);
            var tasks = column.GetTasks();

            BeginTest();

            column.Clear();

            Assert.IsEmpty(column.GetTasks());
            Verify.ObjectIsDeleted(tasks[0]);
            Verify.ObjectIsDeleted(tasks[1]);
        }
    }
}
