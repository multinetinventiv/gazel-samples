using System;
using Inventiv.Todo.Module.TaskManagement.Security;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
	public interface ITaskService
	{
		// PATCH/PUT /tasks/{task_id}
		// Content-Type=application/json
		// {
		//		"name": string
		//		"notes": string
		//		"due_date": string("YYYYMMDDHHmmss")
		// }
		void Update(string name, string notes);
		void Update(DateTime dueDate);
		void Update(string name, string notes, DateTime dueDate);

		// POST /tasks/{task_id}/move
		// Content-Type=x-www-form-urlencoded
		// column_id={column_id}
		void Move(Column column);

		// POST /tasks/{task_id}/assign
		// Content-Type=x-www-form-urlencoded
		// user_id={user_id}
		void Assign(User user);

		// POST /tasks/{task_id}/complete
		void Complete();

		// DELETE /tasks/{task_id}
		void Delete();
	}

	public interface ITasksService
	{
		// GET /tasks/{task_id}
		ITaskInfo GetTask(int taskId);
	}

	public interface ITaskInfo
	{
		int Id { get; }
		string Name { get; }
		DateTime DueDate { get; }
		string Notes { get; }
		bool Completed { get; }
		User User { get; }
	}
}
