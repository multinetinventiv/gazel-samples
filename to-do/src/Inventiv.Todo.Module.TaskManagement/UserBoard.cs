using System.Collections.Generic;
using Gazel;
using Gazel.DataAccess;
using Inventiv.Todo.Module.TaskManagement.Security;
using static Inventiv.Todo.Module.TaskManagement.TaskManagementException;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class UserBoard
    {
        #region IoC

        private readonly IRepository<UserBoard> repository;

        protected UserBoard() { }
        public UserBoard(IRepository<UserBoard> repository)
        {
            this.repository = repository;
        }

        #endregion

        public virtual int Id { get; protected set; }
        public virtual Board Board { get; protected set; }
        public virtual User User { get; protected set; }

        protected internal virtual UserBoard With(Board board, User user)
        {
            if (board == null) { throw new RequiredParameterIsMissing(nameof(board)); }
            if (user == null) { throw new RequiredParameterIsMissing(nameof(user)); }
            if (board.HasUser(user)) { throw new UserAlreadyAddedToBoard(user, board); }

            Board = board;
            User = user;

            repository.Insert(this);

            return this;
        }

        protected internal virtual void Delete() => repository.Delete(this);
    }

    public class UserBoards : Query<UserBoard>
    {
        public UserBoards(IModuleContext context) : base(context) { }

        internal int CountBy(Board board, User user) => CountBy(ub => ub.Board == board && ub.User == user);

        internal UserBoard SingleBy(Board board, User user) => SingleBy(ub => ub.Board == board && ub.User == user);

        internal List<UserBoard> ByBoard(Board board) => By(ub => ub.Board == board);

        internal List<UserBoard> ByUser(User user) => By(ub => ub.User == user);
    }
}