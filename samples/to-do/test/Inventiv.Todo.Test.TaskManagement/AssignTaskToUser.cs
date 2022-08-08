using Inventiv.Todo.Module.TaskManagement;
using Inventiv.ToDo.Test.UnitTest;
using NUnit.Framework;

namespace Inventiv.Todo.Test.UnitTest
{
    [TestFixture]
    public class AssignTaskToUser : ToDoTestBase
    {
        [Test]
        public void GIVEN_there_exists_a_task__WHEN_user_assigns_the_task_to_the_user__THEN_the_user_lists_it_among_its_tasks()
        {
            var user = CreateUser();
            var task = CreateTask(board: CreateBoard(user: user));

            BeginTest();

            task.Assign(user);

            var tasks = user.GetTasks();
            Assert.AreEqual(1, tasks.Count);
            Assert.AreEqual(task.Name, tasks[0].Name);
            Assert.AreEqual(user, task.User);
        }

        [Test]
        public void GIVEN_there_exists_a_task_assigned_to_a_user__WHEN_task_is_assigned_to_another_user__THEN_the_task_is_removed_from_its_former_user()
        {
            var oldUser = CreateUser();
            var newUser = CreateUser();
            var board = CreateBoard(users: new[] { oldUser, newUser });
            var task = CreateTask(assignTo: oldUser, board: board);

            BeginTest();

            task.Assign(newUser);

            Assert.IsEmpty(oldUser.GetTasks());
        }

        [Test]
        public void GIVEN_there_exists_a_user_that_is_not_in_the_board_of_a_task__WHEN_user_assigns_the_task_to_the_user__THEN_system_gives_an_error()
        {
            var user = CreateUser();
            var task = CreateTask();

            BeginTest();

            Assert.Throws<TaskManagementException.UserIsNotAddedToBoard>(() =>
            {
                task.Assign(user);
            });
        }
    }
}
