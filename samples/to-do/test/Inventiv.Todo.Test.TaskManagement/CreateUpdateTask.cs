using Inventiv.Todo.Module.TaskManagement;
using Inventiv.ToDo.Test.UnitTest;
using NUnit.Framework;

namespace Inventiv.Todo.Test.UnitTest
{
    [TestFixture]
    public class CreateUpdateTask : ToDoTestBase
    {
        [Test]
        public void GIVEN_there_exists_a_column__WHEN_user_adds_a_task_to_it__THEN_the_column_lists_it_among_its_tasks()
        {
            var column = CreateColumn();

            BeginTest();

            var task = column.AddTask("task");

            Verify.ObjectIsPersisted(task);
            Assert.AreEqual("task", task.Name);

            var tasks = column.GetTasks();
            Assert.AreEqual(1, tasks.Count);
            Assert.AreEqual(task, tasks[0]);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GIVEN_there_exists_a_column__WHEN_user_adds_a_task_with_an_empty_name__THEN_system_gives_an_error(string name)
        {
            var column = CreateColumn();

            BeginTest();

            Assert.Throws<TaskManagementException.RequiredParameterIsMissing>(() =>
            {
                column.AddTask(name);
            });
        }

        [Test]
        public void GIVEN_there_exists_a_task__WHEN_user_updates_name_and_notes_of_the_task__THEN_task_will_show_its_new_name_and_notes()
        {
            var task = CreateTask();

            BeginTest();

            task.Update("test", "notes");

            Assert.AreEqual("test", task.Name);
            Assert.AreEqual("notes", task.Notes);
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase(" ", " ")]
        public void GIVEN_there_exists_a_task__WHEN_user_updates_the_task_with_an_empty_name_or_notes__THEN_task_will_not_be_updated(string name, string notes)
        {
            var task = CreateTask(taskName: "task", notes: "notes");

            BeginTest();

            Assert.DoesNotThrow(() =>
            {
                task.Update(name, notes);
            });

            Assert.AreEqual("task", task.Name);
            Assert.AreEqual("notes", task.Notes);
        }
    }
}
