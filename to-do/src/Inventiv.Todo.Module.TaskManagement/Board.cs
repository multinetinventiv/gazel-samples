﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gazel;
using Gazel.DataAccess;
using Inventiv.Todo.Module.TaskManagement.Security;
using Inventiv.Todo.Module.TaskManagement.Service;
using static Inventiv.Todo.Module.TaskManagement.TaskManagementException;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class Board : IBoardService, IBoardInfo, IBoardDetail
    {
        #region IoC

        private readonly IRepository<Board> repository;
        private readonly IModuleContext context;

        protected Board() { }
        public Board(IRepository<Board> repository, IModuleContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        #endregion

        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }

        protected internal virtual Board With(string name)
        {
            Update(name);

            repository.Insert(this);

            //you can access current session through context.Session
            //this session object is the one that returned in SecurityManager.GetSession method
            if (context.Session.Account is User user)
            {
                AddUser(user);
            }

            return this;
        }

        public virtual void Update(string name)
        {
            if (name.IsNullOrWhiteSpace()) { throw new RequiredParameterIsMissing(nameof(name)); }

            Name = name;
        }

        public virtual Column AddColumn(string name) => context.New<Column>().With(this, name);

        public virtual List<Column> GetColumns() => context.Query<Columns>().ByBoard(this);

        public virtual List<User> GetUsers() => context.Query<UserBoards>()
                .ByBoard(this)
                .Select(ub => ub.User)
                .ToList();

        public virtual void AddUser(User user) => context.New<UserBoard>().With(this, user);

        public virtual void RemoveUser(User user) => context.Query<UserBoards>().SingleBy(this, user)?.Delete();

        protected internal virtual bool HasUser(User user) => context.Query<UserBoards>().CountBy(this, user) > 0;

        public virtual void Delete()
        {
            GetColumns().ForEach(c => c.Delete());

            repository.Delete(this);
        }

        #region Web Service Mappings

        List<IColumnDetail> IBoardDetail.Columns => GetColumns().Cast<IColumnDetail>().ToList();
        List<IUserInfo> IBoardDetail.Users => GetUsers().Cast<IUserInfo>().ToList();

        IColumnInfo IBoardService.AddColumn(string name) => AddColumn(name);
        List<IColumnInfo> IBoardService.GetColumns() => GetColumns().Cast<IColumnInfo>().ToList();
        List<IUserInfo> IBoardService.GetUsers() => GetUsers().Cast<IUserInfo>().ToList();

        #endregion
    }

    public class Boards : Query<Board>, IBoardsService
    {
        public Boards(IModuleContext context) : base(context) { }

        internal List<Board> ByName(string name) => By(b => true, //default where clause always returns true so that when name is not given it will list all the boards
                When(name).IsNotDefault().ThenAnd(b => b.Name.StartsWith(name)) //a short-cut for conditional query building
            );

        #region Web Service Mappings

        List<IBoardInfo> IBoardsService.GetBoards(string name) => ByName(name).Cast<IBoardInfo>().ToList();
        IBoardDetail IBoardsService.GetBoard(int boardId) => SingleById(boardId);

        #endregion
    }
}

