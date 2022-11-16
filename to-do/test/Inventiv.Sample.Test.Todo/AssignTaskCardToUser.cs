using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class AssignTaskCardToUser : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_task_card__WHEN_user_assigns_the_task_card_to_the_user__THEN_the_user_lists_it_among_its_task_cards()
    {
        var user = CreateUser();
        var taskCard = CreateTaskCard(board: CreateBoard(user: user));

        BeginTest();

        taskCard.Assign(user);

        var taskCards = user.GetTaskCards();
        Assert.AreEqual(1, taskCards.Count);
        Assert.AreEqual(taskCard.Name, taskCards[0].Name);
        Assert.AreEqual(user, taskCard.User);
    }

    [Test]
    public void GIVEN_there_exists_a_task_card_assigned_to_a_user__WHEN_task_card_is_assigned_to_another_user__THEN_the_task_card_is_removed_from_its_former_user()
    {
        var oldUser = CreateUser();
        var newUser = CreateUser();
        var board = CreateBoard(users: new[] { oldUser, newUser });
        var taskCard = CreateTaskCard(assignTo: oldUser, board: board);

        BeginTest();

        taskCard.Assign(newUser);

        Assert.IsEmpty(oldUser.GetTaskCards());
    }

    [Test]
    public void GIVEN_there_exists_a_user_that_is_not_in_the_board_of_a_task_card__WHEN_user_assigns_the_task_card_to_the_user__THEN_system_gives_an_error()
    {
        var user = CreateUser();
        var taskCard = CreateTaskCard();

        BeginTest();

        Assert.Throws<TodoExceptions.UserIsNotAddedToBoard>(() =>
        {
            taskCard.Assign(user);
        });
    }
}
