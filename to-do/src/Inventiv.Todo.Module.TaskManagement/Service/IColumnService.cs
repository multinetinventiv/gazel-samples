using System.Collections.Generic;
using Gazel.DataAccess;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
	public interface IColumnService
	{
		// PATCH /columns/{id}
		// Content-Type = applications/json
		// {
		//		"name": "{name}"
		// }
		void Update(string name);

		// POST /columns/{column_id}/tasks
		// Content-Type = applications/json
		// {
		//		"name": "{name}"
		// }
		ITaskInfo AddTask(string name);

		// GET /columns/{column_id}/tasks
		List<ITaskInfo> GetTasks();
		
		// POST /columns/{column_id}/clear
		void Clear();
		
		// DELETE /columns/{column_id} 
		void Delete();

		// POST /columns/{column_id}/move_tasks_and_delete
		// Content-Type = x-www-form-urlencoded
		// target_column_id={target_column_id}
		void MoveTasksAndDelete(Column targetColumn);
	}

	public interface IColumnsService : IQuery
	{
		// GET /columns/{column_id}
		IColumnDetail GetColumn(int columnId);
	}

	public interface IColumnInfo
	{
		int Id { get; }
		string Name { get; }
		Board Board { get; }
	}
	
	public interface IColumnDetail
	{
		int Id { get; }
		string Name { get; }
		List<ITaskInfo> Tasks { get; } //Eager fetching in web service layer
	}
}
