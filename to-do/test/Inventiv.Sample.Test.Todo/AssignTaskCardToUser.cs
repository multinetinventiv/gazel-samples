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
        Assert.That(taskCards.Count, Is.EqualTo(1));
        Assert.That(taskCards[0].Name, Is.EqualTo(taskCard.Name));
        Assert.That(taskCard.User, Is.EqualTo(user));
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

        Assert.That(oldUser.GetTaskCards(), Is.Empty);
    }

    [Test]
    public void GIVEN_there_exists_a_user_that_is_not_in_the_board_of_a_task_card__WHEN_user_assigns_the_task_card_to_the_user__THEN_system_gives_an_error()
    {
        var user = CreateUser();
        var taskCard = CreateTaskCard();

        BeginTest();

        Assert.That(() => taskCard.Assign(user), Throws.TypeOf<TodoExceptions.UserIsNotAddedToBoard>());
    }
}
