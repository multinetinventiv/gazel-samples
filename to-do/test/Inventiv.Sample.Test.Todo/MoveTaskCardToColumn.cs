using Inventiv.Sample.Module.Todo;

namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class MoveTaskCardToColumn : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_task_card__WHEN_user_moves_the_task_card_to_a_column__THEN_it_is_removed_from_its_old_column_and_listed_in_its_new_column()
    {
        var board = CreateBoard();
        var taskCard = CreateTaskCard(board: board);
        var toColumn = CreateColumn(board: board);

        BeginTest();

        taskCard.Move(toColumn);

        Assert.AreEqual(taskCard.Name, toColumn.GetTaskCards()[0].Name);
    }

    [Test]
    public void GIVEN_there_exists_a_task_card_and_a_column_in_a_different_board__WHEN_user_moves_the_task_card_to_the_column__THEN_system_gives_an_error()
    {
        var taskCard = CreateTaskCard();
        var targetColumn = CreateColumn();

        BeginTest();

        Assert.Throws<TodoExceptions.ColumnDoesNotBelongToBoard>(() => taskCard.Move(targetColumn));
    }
}
