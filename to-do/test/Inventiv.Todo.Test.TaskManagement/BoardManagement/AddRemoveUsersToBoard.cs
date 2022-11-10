using Inventiv.Todo.Module.TaskManagement;
using NUnit.Framework;

namespace Inventiv.Todo.Test.TaskManagement.BoardManagement
{
    [TestFixture]
    public class AddRemoveUsersToBoard : ToDoTestBase
    {
        [Test]
        public void GIVEN_there_exists_a_board_and_a_user__WHEN_the_user_is_added_to_the_board__THEN_the_board_lists_the_user_in_its_users_and_vice_versa()
        {
            var board = CreateBoard();
            var user = CreateUser();

            BeginTest();

            board.AddUser(user);

            var boardUsers = board.GetUsers();

            Assert.AreEqual(1, boardUsers.Count);
            Assert.AreEqual(user, boardUsers[0]);

            var userBoards = user.GetBoards();

            Assert.AreEqual(1, userBoards.Count);
            Assert.AreEqual(board, userBoards[0]);
        }

        [Test]
        public void GIVEN_there_exists_a_board_with_a_user__WHEN_the_same_user_is_added_to_the_board__THEN_system_gives_an_error()
        {
            var user = CreateUser();
            var board = CreateBoard(user: user);

            BeginTest();

            Assert.Throws<TaskManagementException.UserAlreadyAddedToBoard>(() =>
            {
                board.AddUser(user);
            });
        }

        [Test]
        public void GIVEN_there_exists_a_board__WHEN_user_adds_null_user_to_the_board__THEN_system_gives_an_error()
        {
            var board = CreateBoard();

            BeginTest();

            Assert.Throws<TaskManagementException.RequiredParameterIsMissing>(() => board.AddUser(null));
        }

        [Test]
        public void GIVEN_there_exists_a_board_with_two_users__WHEN_user_gets_the_board__THEN_the_board_lists_its_two_users()
        {
            var board = CreateBoard(users: new[]
            {
                CreateUser(name:"user 1"),
                CreateUser(name:"user 2")
            });

            BeginTest();

            var userList = board.GetUsers();

            Assert.AreEqual(2, userList.Count);
            Assert.AreEqual("user 1", userList[0].Name);
            Assert.AreEqual("user 2", userList[1].Name);
        }

        [Test]
        public void GIVEN_there_exists_a_board_with_a_user__WHEN_the_user_is_removed_from_the_board__THEN_the_board_does_not_list_that_user_any_more()
        {
            var user = CreateUser();
            var board = CreateBoard(user: user);

            BeginTest();

            board.RemoveUser(user);

            Assert.IsEmpty(board.GetUsers());
        }

        [Test]
        public void GIVEN_there_exists_a_board_without_a_user__WHEN_user_tries_to_remove_a_user_from_the_board__THEN_system_does_not_give_an_error()
        {
            var user = CreateUser();
            var board = CreateBoard();

            BeginTest();

            Assert.DoesNotThrow(() =>
            {
                board.RemoveUser(user);
            });
        }
    }
}
