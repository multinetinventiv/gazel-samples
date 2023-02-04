using Gazel;
using Moq;
using Inventiv.Sample.Module.Todo.Jobs;

namespace Inventiv.Sample.Test.Todo;

[TestFixture]
public class RemindTaskCardToUser : ToDoTestBase
{
    [Test]
    public void GIVEN_there_exists_a_task_card__WHEN_user_sets_a_due_date_to_the_task_card__THEN_assigned_user_will_be_reminded_at_given_date()
    {
        var taskCard = CreateTaskCard(
            taskCardName: "test task",
            assignTo: CreateUser(email: "test@gazel.io")
        );

        BeginTest();

        taskCard.Update(dueDate: Tomorrow());

        SetUpTime(Tomorrow().Date);

        Context.New<ReminderJob>().Execute();

        _mockMailClient.Verify(
            ms => ms.Send(
                "test@gazel.io",
                It.Is<string>(sbj => sbj.Contains("test task")),
                It.Is<string>(msg => msg.Contains("test task"))
            ),
            Times.Once()
        );
    }

    [Test]
    public void GIVEN_user_is_reminded_about_a_task_card__WHEN_reminder_job_executes_a_second_time__THEN_the_user_will_not_be_reminded_again()
    {
        CreateTaskCard(
            taskCardName: "test task",
            assignTo: CreateUser(email: "test@gazel.io"),
            remindedAt: Now()
        );

        BeginTest();

        Context.New<ReminderJob>().Execute();

        _mockMailClient.Verify(
            ms => ms.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once()
        );
    }

    [Test]
    public void GIVEN_user_is_reminded_about_a_task_card__WHEN_user_updates_due_date__THEN_the_user_will_be_reminded_again_at_due_date()
    {
        var taskCard = CreateTaskCard(
            taskCardName: "test task",
            assignTo: CreateUser(email: "test@gazel.io"),
            remindedAt: Now()
        );

        SetUpTime(Tomorrow());

        BeginTest();

        taskCard.Update(dueDate: Tomorrow());

        SetUpTime(Tomorrow());

        Context.New<ReminderJob>().Execute();

        _mockMailClient.Verify(
            ms => ms.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Exactly(2)
        );
    }
}
