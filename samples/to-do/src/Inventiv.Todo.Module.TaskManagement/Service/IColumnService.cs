using System.Collections.Generic;
using Gazel.DataAccess;

namespace Inventiv.Todo.Module.TaskManagement.Service
{
    public interface IColumnService
    {
        // PATCH /columns/{id}
        // ContentType = applications/json
        // {
        //  "name": string
        // }
        void Update(string name);

        // POST /columns/{id}/tasks
        // ContentType = applications/json
        // {
        //  "name": string
        // }
        ITaskInfo AddTask(string name);

        // GET /columns/{id}/tasks
        List<ITaskInfo> GetTasks();

        // POST /columns/{id}/clear
        void Clear();

        // DELETE /columns/{id} 
        void Delete();

        // POST /columns/{id}/move-tasks-and-delete
        // ContentType = application/json
        // {
        //  "targetColumnId": int
        // }
        void MoveTasksAndDelete(Column targetColumn);
    }

    public interface IColumnsService : IQuery
    {
        // GET /columns/{id}
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
        List<ITaskInfo> Tasks { get; } // Eager fetching in web service layer
    }
}
