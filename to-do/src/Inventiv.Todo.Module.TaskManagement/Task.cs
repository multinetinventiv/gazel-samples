using System;
using System.Collections.Generic;
using System.Linq;
using Gazel;
using Gazel.DataAccess;
using Inventiv.Todo.Module.TaskManagement.Mail;
using Inventiv.Todo.Module.TaskManagement.Security;
using Inventiv.Todo.Module.TaskManagement.Service;

namespace Inventiv.Todo.Module.TaskManagement
{
	public class Task : ITaskService, ITaskInfo
	{
		#region IoC

		private readonly IRepository<Task> repository;
		private readonly IMailService mailService;
		private readonly IModuleContext context;

		protected Task() { }
		public Task(IRepository<Task> repository, IMailService mailService, IModuleContext context)
		{
			this.repository = repository;
			this.mailService = mailService;
			this.context = context;
		}

		#endregion

		public virtual int Id { get; protected set; }
		public virtual string Name { get; protected set; }
		public virtual Column Column { get; protected set; }
		public virtual string Notes { get; protected set; }
		public virtual bool Completed { get; protected set; }
		public virtual User User { get; protected set; }
		public virtual DateTime DueDate { get; protected set; }
		public virtual bool Reminded { get; protected set; }

		protected internal virtual Task With(Column column, string name)
		{
			if (string.IsNullOrWhiteSpace(name)) { throw new TaskManagementException.RequiredParameterIsMissing(nameof(name)); }

			Column = column ?? throw new TaskManagementException.RequiredParameterIsMissing(nameof(column));
			Name = name;
			DueDate = context.System.Now.AddDays(7);

			repository.Insert(this);

			return this;
		}

		public virtual void Update(string name, string notes) { Update(name, notes, default(DateTime)); }
		public virtual void Update(DateTime dueDate) { Update(null, null, dueDate); }
		public virtual void Update(string name, string notes, DateTime dueDate)
		{
			if (name.IsNullOrWhiteSpace()) { name = Name; }
			if (notes.IsNullOrWhiteSpace()) { notes = Notes; }
			if (dueDate.IsDefault()) { dueDate = DueDate; }

			Name = name;
			Notes = notes;
			DueDate = dueDate;

			if (DueDate > context.System.Now)
			{
				Reminded = false;
			}
		}

		public virtual void Complete()
		{
			Completed = true;
			Reminded = true;
		}

		protected internal virtual void RemindUser()
		{
			if (User != null && !User.Email.IsDefault())
			{
				mailService.Send(
					User.Email.Value,
					$"Reminder of task: {Name}",
					$"{Name}<br/><br/>{Notes}"
				);
			}

			Reminded = true;
		}

		public virtual void Move(Column column)
		{
			Column.ValidateMovement(column);

			Column = column;
		}

		public virtual void Assign(User user)
		{
			if (user == null) { throw new TaskManagementException.RequiredParameterIsMissing(nameof(user)); }

			if (!Column.Board.HasUser(user))
			{
				throw new TaskManagementException.UserIsNotAddedToBoard(user, Column.Board);
			}

			User = user;
		}

		public virtual void Delete()
		{
			repository.Delete(this);
		}
	}

	public class Tasks : Query<Task>, ITasksService
	{
		public Tasks(IModuleContext context) : base(context) { }

		internal List<Task> ByColumn(Column column)
		{
			return By(t => t.Column == column);
		}

		internal List<Task> ByUser(User user)
		{
			return By(t => t.User == user);
		}

		internal List<Task> TakeBy(int maxRowCount, DateTime maxDueDate, bool reminded)
		{
			return By(t => t.DueDate < maxDueDate && t.Reminded == reminded,
				take: maxRowCount
			);
		}

		#region Web Service Mappings

		ITaskInfo ITasksService.GetTask(int taskId) => SingleById(taskId);

		#endregion
	}
}