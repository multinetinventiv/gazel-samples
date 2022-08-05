using System;
using System.Linq;
using Castle.MicroKernel;
using Inventiv.Todo.Module.TaskManagement;
using Inventiv.Todo.Module.TaskManagement.Jobs;
using Inventiv.Todo.Module.TaskManagement.Mail;
using Inventiv.Todo.Module.TaskManagement.Security;
using Gazel.UnitTesting;
using Moq;
using Gazel;

namespace Inventiv.ToDo.Test.UnitTest
{
    /// <summary>
    /// Base class for all test fixtures. Includes setup and create helper methods to make test cases cleaner.
    /// </summary>
    public abstract class ToDoTestBase : TestBase
    {
        protected SecurityManager securityManager;
        protected TaskManager taskManager;
        protected Mock<IMailService> mockMailService;
        
        static ToDoTestBase()
        {
            Config.RootNamespace = "Inventiv";
        }

        public override void SetUp()
        {
            base.SetUp();

            mockMailService = CreateMock<IMailService>(true);

            securityManager = Context.Get<SecurityManager>();
            taskManager = Context.Get<TaskManager>();

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

            //When user is created, password value remains plain.
            //This causes nhibernate to make an update to user table, because it thinks this user object is dirty.
            //We need to reload it from database so that password value becomes hashed.
            //This way we get rid of that extra update.
            //Comment out next line to see how it causes unnecessary updates.
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

            var result = securityManager.Login(user.Email, Password.Parse("1234"));

            if (expired)
            {
                SetUpTime(Context.System.Now.AddDays(1).AddMilliseconds(1));
            }

            return result;
        }

        protected Board CreateBoard(
            string boardName = "board",
            User user = null, User[] users = null
        )
        {
            var result = Context.Get<TaskManager>().CreateBoard(boardName);

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
            string boardName = "board", Board board = null,
            int taskCount = 0, string[] taskNames = null
        )
        {
            if (board == null)
            {
                board = CreateBoard(boardName: boardName);
            }

            var result = board.AddColumn(columnName);

            if (taskCount > 0 && taskNames != null && taskNames.Length > 0 && taskCount != taskNames.Length)
            {
                throw new InvalidOperationException(
                    $"Both taskCount({taskCount}) and taskNames({taskNames.Length}) are given and they are different. " +
                    $"Setting one of them is sufficient to create a column."
                );
            }

            if (taskNames == null || taskNames.Length == 0)
            {
                taskNames = Enumerable.Range(0, taskCount).Select(i => "task " + i).ToArray();
            }

            foreach (var taskName in taskNames)
            {
                result.AddTask(taskName);
            }

            return result;
        }

        protected Task CreateTask(
            string taskName = "task",
            string columnName = "column", Column column = null,
            string boardName = "board", Board board = null,
            User assignTo = null,
            DateTime? remindedAt = null,
            string notes = null)
        {
            if (column == null)
            {
                column = CreateColumn(columnName: columnName, boardName: boardName, board: board);
            }

            var result = column.AddTask(taskName);

            if (notes != null)
            {
                result.Update(taskName, notes);
            }

            if (assignTo != null)
            {
                Try(() => column.Board.AddUser(assignTo));

                result.Assign(assignTo);
            }


            if (remindedAt != null)
            {
                result.Update(remindedAt.Value);

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
}