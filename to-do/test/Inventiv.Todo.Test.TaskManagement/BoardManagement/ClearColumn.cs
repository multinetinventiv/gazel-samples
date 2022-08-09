using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement.TaskManagement.BoardManagement
{
    [TestFixture]
    public class ClearColumn : ToDoTestBase
    {
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
