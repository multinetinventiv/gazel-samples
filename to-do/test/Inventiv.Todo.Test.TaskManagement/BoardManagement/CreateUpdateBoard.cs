using Inventiv.Todo.Module.TaskManagement;
using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement.TaskManagement.BoardManagement
{
    [TestFixture]
    public class CreateUpdateBoard : ToDoTestBase
    {
        [Test]
        public void GIVEN_in_any_condition__WHEN_user_creates_a_new_board__THEN_the_board_is_created_with_given_name()
        {
            BeginTest();

            var board = taskManager.CreateBoard("board");

            Verify.ObjectIsPersisted(board);
            Assert.AreEqual("board", board.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GIVEN_in_any_condition__WHEN_user_creates_a_new_board_with_an_empty_name__THEN_system_gives_an_error(string name)
        {
            BeginTest();

            Assert.Throws<TaskManagementException.RequiredParameterIsMissing>(() =>
            {
                CreateBoard(name);
            });
        }

        [Test]
        public void GIVEN_user_is_anonymous__WHEN_user_creates_a_new_board__THEN_the_board_is_created_without_any_users_in_it()
        {
            SetUpSession(CreateAnonymousSession());

            BeginTest();

            var board = taskManager.CreateBoard(AString());

            Assert.AreEqual(0, board.GetUsers().Count);
        }

        [Test]
        public void GIVEN_a_user_is_logged_in__WHEN_user_creates_a_new_board__THEN_the_board_is_automatically_added_to_the_user()
        {
            SetUpSession(CreateUserSession());

            BeginTest();

            var board = taskManager.CreateBoard(AString());
            var users = board.GetUsers();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(Context.Session.Account, users[0]);
        }

        [Test]
        public void GIVEN_there_exists_a_board__WHEN_user_updates_its_name__THEN_the_board_will_be_listed_with_its_new_name()
        {
            var board = CreateBoard();

            BeginTest();

            board.Update("newBoard");

            Assert.AreEqual("newBoard", board.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GIVEN_there_exists_a_board__WHEN_user_updates_its_name_with_an_empty_name__THEN_system_gives_an_error(string name)
        {
            var board = CreateBoard();

            BeginTest();

            Assert.Throws<TaskManagementException.RequiredParameterIsMissing>(() =>
            {
                board.Update(name);
            });
        }
    }
}
