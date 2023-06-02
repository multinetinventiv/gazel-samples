using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo.BoardManagement;

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

        Assert.That(boardUsers.Count, Is.EqualTo(1));
        Assert.That(boardUsers[0], Is.EqualTo(user));

        var userBoards = user.GetBoards();

        Assert.That(userBoards.Count, Is.EqualTo(1));
        Assert.That(userBoards[0], Is.EqualTo(board));
    }

    [Test]
    public void GIVEN_there_exists_a_board_with_a_user__WHEN_the_same_user_is_added_to_the_board__THEN_system_gives_an_error()
    {
        var user = CreateUser();
        var board = CreateBoard(user: user);

        BeginTest();

        Assert.That(() => board.AddUser(user), Throws.TypeOf<TodoExceptions.UserAlreadyAddedToBoard>());
    }

    [Test]
    public void GIVEN_there_exists_a_board__WHEN_user_adds_null_user_to_the_board__THEN_system_gives_an_error()
    {
        var board = CreateBoard();

        BeginTest();

        Assert.That(() => board.AddUser(null!), Throws.TypeOf<TodoExceptions.RequiredParameterIsMissing>());
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

        Assert.That(userList.Count, Is.EqualTo(2));
        Assert.That(userList[0].Name, Is.EqualTo("user 1"));
        Assert.That(userList[1].Name, Is.EqualTo("user 2"));
    }

    [Test]
    public void GIVEN_there_exists_a_board_with_a_user__WHEN_the_user_is_removed_from_the_board__THEN_the_board_does_not_list_that_user_any_more()
    {
        var user = CreateUser();
        var board = CreateBoard(user: user);

        BeginTest();

        board.RemoveUser(user);

        Assert.That(board.GetUsers(), Is.Empty);
    }

    [Test]
    public void GIVEN_there_exists_a_board_without_a_user__WHEN_user_tries_to_remove_a_user_from_the_board__THEN_system_does_not_give_an_error()
    {
        var user = CreateUser();
        var board = CreateBoard();

        BeginTest();

        Assert.That(() => board.RemoveUser(user), Throws.Nothing);
    }
}
