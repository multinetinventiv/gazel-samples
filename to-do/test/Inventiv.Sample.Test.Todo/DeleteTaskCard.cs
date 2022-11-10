namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class DeleteTaskCard : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_task_card__WHEN_user_deletes_the_task_card__THEN_then_it_is_removed_from_system()
    {
        var taskCard = CreateTaskCard();

        BeginTest();

        taskCard.Delete();

        Verify.ObjectIsDeleted(taskCard);
    }
}
