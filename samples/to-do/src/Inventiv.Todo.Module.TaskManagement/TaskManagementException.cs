using Gazel.Service;
using Inventiv.Todo.Module.TaskManagement.Security;

namespace Inventiv.Todo.Module.TaskManagement
{
    // Exceptions and error codes can be easily duplicated when they are hard to find.
    // This nested class structure makes it easy to find an exception class to avoid duplicates.
    // This structure is optional, Gazel only requires you to extend ServiceException
    public static class TaskManagementException
    {
        public class UserAlreadyAddedToBoard : ServiceException // ServiceException makes this exception a handled exception
        {
            public UserAlreadyAddedToBoard(User user, Board board)
                : base(
                    ResultCodes.TaskManagement.Err(0), // Using result codes helper class to create a unique error code
                    user.Name, board.Name // These parameters will be passed to localization when creating an error message
                )
            { }
        }

        public class UserIsNotAddedToBoard : ServiceException
        {
            public UserIsNotAddedToBoard(User user, Board board)
                : base(ResultCodes.TaskManagement.Err(1), user.Name, board.Name) { }
        }

        public class ColumnDoesNotBelongToBoard : ServiceException
        {
            public ColumnDoesNotBelongToBoard(Column column, Board board)
                : base(ResultCodes.TaskManagement.Err(2), column.Name, board.Name) { }
        }

        public class RequiredParameterIsMissing : ServiceException
        {
            public RequiredParameterIsMissing(string name)
                : base(ResultCodes.TaskManagement.Err(3), name) { }
        }
    }
}