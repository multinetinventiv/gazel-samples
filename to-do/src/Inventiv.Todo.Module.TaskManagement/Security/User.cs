using System;
using System.Collections.Generic;
using System.Linq;
using Gazel;
using Gazel.DataAccess;
using Gazel.Security;
using Inventiv.Todo.Module.TaskManagement.Service;
using static Inventiv.Todo.Module.TaskManagement.TaskManagementException;

namespace Inventiv.Todo.Module.TaskManagement.Security
{
    public class User : IUserService, IUserInfo, IAccount //User implements IAccount interface so that it can be used by Gazel for authentication and authorization
    {
        #region IoC

        private readonly IRepository<User> repository;
        private readonly IModuleContext context;

        protected User() { }
        public User(IRepository<User> repository, IModuleContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        #endregion

        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Email Email { get; protected set; }
        public virtual Password Password { get; protected set; }

        protected internal virtual User With(string name, Email email, Password password)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new RequiredParameterIsMissing(nameof(name)); }
            if (email.IsDefault()) { throw new RequiredParameterIsMissing(nameof(email)); }
            if (password.IsDefault()) { throw new RequiredParameterIsMissing(nameof(password)); }

            Name = name;
            Email = email;
            Password = password;

            repository.Insert(this);

            return this;
        }

        protected internal virtual UserSession CreateNewSession() => context.New<UserSession>().With(this);

        public virtual List<Task> GetTasks() => context.Query<Tasks>().ByUser(this);

        public virtual List<Board> GetBoards()
        {
            return context.Query<UserBoards>()
                .ByUser(this)
                .Select(ub => ub.Board)
                .ToList();
        }

        #region Api Mappings

        #region Security

        string IAccount.DisplayName => Name;
        bool IAccount.HasAccess(IResource resource) => true;

        #endregion

        #endregion

        #region Web Service Mappings

        List<ITaskInfo> IUserService.GetTasks() => GetTasks().ToList<ITaskInfo>();
        List<IBoardInfo> IUserService.GetBoards() => GetBoards().ToList<IBoardInfo>();

        #endregion
    }

    public class Users : Query<User>, IUsersService
    {
        public Users(IModuleContext context) : base(context) { }

        internal User SingleBy(Email email, Password password) => SingleBy(u => u.Email == email && u.Password == password);

        internal List<User> ByName(string name) => By(b => true, //default where clause always returns true so that when name is not given it will list all the users
                When(name).IsNotDefault().ThenAnd(b => b.Name.StartsWith(name)) //a short-cut for conditional query building
            );

        #region Web Service Mappings

        List<IUserInfo> IUsersService.GetUsers(string name) => ByName(name).ToList<IUserInfo>();
        IUserInfo IUsersService.GetUser(int userId) => SingleById(userId);

        #endregion
    }
}
