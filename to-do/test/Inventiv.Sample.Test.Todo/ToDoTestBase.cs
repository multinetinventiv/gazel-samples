using Castle.MicroKernel;
using Gazel;
using Gazel.UnitTesting;
using Moq;
using Inventiv.Sample.Module.Todo;
using Inventiv.Sample.Module.Todo.Jobs;
using Inventiv.Sample.Module.Todo.Mail;
using Inventiv.Sample.Module.Todo.Security;

namespace Inventiv.Sample.Test.Todo;

/// <summary>
/// Base class for all test fixtures. Includes setup and create helper methods to make test cases cleaner.
/// </summary>
public abstract class ToDoTestBase : TestBase
{
    protected SecurityManager _securityManager = default!;
    protected TodoManager _todoManager = default!;
    protected Mock<IMailClient> _mockMailClient = default!;

    static ToDoTestBase()
    {
        Config.RootNamespace = "Inventiv";
    }

    public override void SetUp()
    {
        base.SetUp();

        _mockMailClient = CreateMock<IMailClient>(true);

        _securityManager = Context.Get<SecurityManager>();
        _todoManager = Context.Get<TodoManager>();

        SetUpSession(CreateAnonymousSession());
    }

    protected AnonymousSession CreateAnonymousSession() => Context.New<AnonymousSession>();

    protected User CreateUser(
        string name = "User",
        string email = "test@gazel.io",
        string password = "1234"
    )
    {
        var emailByType = Email.Parse(email);
        var passwordByType = Password.Parse(password);

        var user = Context.Get<SecurityManager>().CreateUser(name, emailByType, passwordByType);

        // When user is created, password value remains plain.
        // This causes nhibernate to make an update to user table, because it thinks this user object is dirty.
        // We need to reload it from database so that password value becomes hashed.
        // This way we get rid of that extra update.
        // Comment out next line to see how it causes unnecessary updates.
        Context.Get<IKernel>().Resolve<NHibernate.ISession>().Refresh(user);

        return user;
    }

    protected UserSession CreateUserSession(bool expired = false)
    {
        var user = CreateUser(password: "1234");

        if (expired)
        {
            SetUpTime(Context.System.Now.AddDays(-1).AddMilliseconds(-1));
        }

        var result = _securityManager.Login(user.Email, Password.Parse("1234"));

        if (expired)
        {
            SetUpTime(Context.System.Now.AddDays(1).AddMilliseconds(1));
        }

        return result;
    }

    protected Board CreateBoard(
        string boardName = "board",
        User? user = null, User[]? users = null
    )
    {
        var result = Context.Get<TodoManager>().CreateBoard(boardName);

        if (user != null)
        {
            result.AddUser(user);
        }

        if (users != null)
        {
            foreach (var user_ in users)
            {
                result.AddUser(user_);
            }
        }

        return result;
    }

    protected Column CreateColumn(
        string columnName = "column",
        string boardName = "board", Board? board = null,
        int taskCardCount = 0, string[]? taskCardNames = null
    )
    {
        if (board == null)
        {
            board = CreateBoard(boardName: boardName);
        }

        var result = board.AddColumn(columnName);

        if (taskCardCount > 0 && taskCardNames != null && taskCardNames.Length > 0 && taskCardCount != taskCardNames.Length)
        {
            throw new InvalidOperationException(
                $"Both taskCardCount({taskCardCount}) and taskCardNames({taskCardNames.Length}) are given and they are different. " +
                $"Setting one of them is sufficient to create a column."
            );
        }

        if (taskCardNames == null || taskCardNames.Length == 0)
        {
            taskCardNames = Enumerable.Range(0, taskCardCount).Select(i => "task " + i).ToArray();
        }

        foreach (var taskCardName in taskCardNames)
        {
            result.AddTaskCard(taskCardName);
        }

        return result;
    }

    protected TaskCard CreateTaskCard(
        string taskCardName = "task",
        string columnName = "column", Column? column = null,
        string boardName = "board", Board? board = null,
        User? assignTo = null,
        DateTime? remindedAt = null,
        string? notes = null)
    {
        if (column == null)
        {
            column = CreateColumn(columnName: columnName, boardName: boardName, board: board);
        }

        var result = column.AddTaskCard(taskCardName);

        if (notes != null)
        {
            result.Update(taskCardName, notes);
        }

        if (assignTo != null)
        {
            Try(() => column.Board.AddUser(assignTo));

            result.Assign(assignTo);
        }


        if (remindedAt != null)
        {
            result.Update(dueDate: remindedAt.Value);

            var oldTime = Context.System.Now;

            SetUpTime(remindedAt.Value.Date);

            Context.New<ReminderJob>().Execute();

            SetUpTime(oldTime);
        }
        return result;
    }

    protected string AString() => "test string";

    protected Password APassword() => Password.Parse("test pwd");

    protected DateTime Now() => Context.System.Now;

    protected DateTime Tomorrow() => Context.System.Now.AddDays(1);

    protected void Try(Action action)
    {
        try
        {
            action();
        }
        catch
        {
            // ignored
        }
    }
}
