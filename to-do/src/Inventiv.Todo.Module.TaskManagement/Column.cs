using System;
using System.Collections.Generic;
using System.Linq;
using Gazel;
using Gazel.DataAccess;
using Inventiv.Todo.Module.TaskManagement.Service;
using static Inventiv.Todo.Module.TaskManagement.TaskManagementException;

namespace Inventiv.Todo.Module.TaskManagement
{
    public class Column : IColumnService, IColumnInfo, IColumnDetail
    {
        #region IoC

        private readonly IRepository<Column> repository;
        private readonly IModuleContext context;

        protected Column() { }
        public Column(IRepository<Column> repository, IModuleContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        #endregion

        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Board Board { get; protected set; }

        protected internal virtual Column With(Board board, string name)
        {
            Board = board ?? throw new RequiredParameterIsMissing(nameof(board));

            Update(name);

            repository.Insert(this);

            return this;
        }

        public virtual void Update(string name)
        {
            if (name.IsNullOrWhiteSpace()) { throw new RequiredParameterIsMissing(nameof(name)); }

            Name = name;
        }

        public virtual Task AddTask(string name) => context.New<Task>().With(this, name);

        public virtual List<Task> GetTasks() => context.Query<Tasks>().ByColumn(this);

        public virtual void Delete()
        {
            Clear();

            repository.Delete(this);
        }

        public virtual void Clear() => GetTasks().ForEach(t => t.Delete());

        public virtual void MoveTasksAndDelete(Column targetColumn)
        {
            ValidateMovement(targetColumn);

            GetTasks().ForEach(t => t.Move(targetColumn));

            Delete();
        }

        protected internal virtual void ValidateMovement(Column targetColumn)
        {
            if (targetColumn == null) { throw new RequiredParameterIsMissing(nameof(targetColumn)); }
            if (Board != targetColumn.Board) { throw new ColumnDoesNotBelongToBoard(targetColumn, Board); }
        }

        #region Web Service Mappings

        List<ITaskInfo> IColumnDetail.Tasks => GetTasks().Cast<ITaskInfo>().ToList();

        List<ITaskInfo> IColumnService.GetTasks() => GetTasks().Cast<ITaskInfo>().ToList();
        ITaskInfo IColumnService.AddTask(string name) => AddTask(name);

        #endregion
    }

    public class Columns : Query<Column>, IColumnsService
    {
        public Columns(IModuleContext context) : base(context) { }

        internal List<Column> ByBoard(Board board) => By(c => c.Board == board);

        internal List<Column> ByName(string name) => By(b => true,
                When(name).IsNotDefault().ThenAnd(b => b.Name.StartsWith(name))
            );

        #region Web Service Mappings

        IColumnDetail IColumnsService.GetColumn(int columnId) => SingleById(columnId);

        #endregion
    }
}